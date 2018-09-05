using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgahClassLibrary
{
    public static class UtitlityCodes
    {
        public static string ByteToHumanReadableSize(long byteCount)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = byteCount;
            var order = 0;
                while (byteCount >= 1024 && order < sizes.Length - 1)
                        {
                            order++;
                            len = len / 1024;
                        }
                return $"{len:0.##} {sizes[order]}";
        }


        /// <summary>
        /// ExecuteAndReturnExecutionTimeSpan(() => MethodToExecute());
        /// </summary>
        /// <param name="action">MethodName</param>
        /// <returns></returns>
        public static TimeSpan ExecuteAndReturnExecutionTimeSpan(Action action)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;

        }
    }
    
}
