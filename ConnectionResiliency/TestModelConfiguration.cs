using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.Entity.SqlServer.Utilities;
using System.Data.SqlClient;
using System.Threading;


namespace ConnectionResiliency
{
    public class TestModelConfiguration:DbConfiguration
    {
        public TestModelConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new MyCustomExecutionStrategy());
        }
    }

    public class MyCustomExecutionStrategy : IDbExecutionStrategy
    {
        public void Execute(Action operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            this.Execute<object>((Func<object>)(() =>
            {
                operation();
                return (object)null;
            }));
        }


        public TResult Execute<TResult>(Func<TResult> operation)
        {
            Check.NotNull<Func<TResult>>(operation, nameof(operation));
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                if (DbExecutionStrategy.UnwrapAndHandleException<bool>(ex, new Func<Exception, bool>(ShouldRetryOn)))
                    throw new EntityException("TransientExceptionDetected", ex);
                throw;
            }
        }

        public Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
        {
            Check.NotNull<Func<Task>>(operation, nameof(operation));
            cancellationToken.ThrowIfCancellationRequested();
            return (Task)ExecuteAsyncImplementation<bool>((Func<Task<bool>>)(async () =>
            {
                await operation().ConfigureAwait(false);
                return true;
            }));
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            Check.NotNull<Func<Task<TResult>>>(operation, nameof(operation));
            cancellationToken.ThrowIfCancellationRequested();
            return ExecuteAsyncImplementation<TResult>(operation);
        }

        public bool RetriesOnFailure
        {
            get
            {
                return true;
            }
        }

        public static bool ShouldRetryOn(Exception ex)
        {
            SqlException sqlException = ex as SqlException;
            if (sqlException != null)
            {
                foreach (SqlError error in sqlException.Errors)
                {
                    switch (error.Number)
                    {
                        case -2:            //TimeOut Occured.
                        case -1:            //The Server was not found or was not accessible.

                        case 201:           // Stored procedure expects parameters which was not supplied.
                        case 207:           //Invalid Column name.:
                        case 208:           // Invalid Object name.(Table or Database)

                        case 2228:          // Could not find Stored procedure.

                        case 4060:          //Cannot open database.
                        case 18456:         //Login failed for user.

                            return false;
                        default:
                            continue;
                    }
                }
                return true;
            }
            return ex is TimeoutException;
        }
        private static async Task<TResult> ExecuteAsyncImplementation<TResult>(Func<Task<TResult>> func)
        {
            TResult result;
            try
            {
                result = await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (DbExecutionStrategy.UnwrapAndHandleException<bool>(ex, new Func<Exception, bool>(ShouldRetryOn)))
                    throw new EntityException("TransientExceptionDetected", ex);
                throw;
            }
            return result;
        }

    }
}


public class Check
{
    public static T NotNull<T>(T value, string parameterName) where T : class
    {
        if ((object)value == null)
            throw new ArgumentNullException(parameterName);
        return value;
    }
    public static T? NotNull<T>(T? value, string parameterName) where T : struct
    {
        if (!value.HasValue)
            throw new ArgumentNullException(parameterName);
        return value;
    }
}