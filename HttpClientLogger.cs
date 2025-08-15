using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using Dapper;
using System.Collections.Concurrent;

// Log model
public record HttpCallLogModel
{
public Guid Id { get; init; } = Guid.NewGuid();
public string CorrelationId { get; init; } = string.Empty;
public DateTime StartTime { get; init; }
public DateTime? EndTime { get; init; }
public TimeSpan? Duration { get; init; }

```
public string Method { get; init; } = string.Empty;
public string Host { get; init; } = string.Empty;
public string Path { get; init; } = string.Empty;
public string? RequestHeaders { get; init; }
public string? RequestBody { get; init; }
public string? RequestContentType { get; init; }
public long? RequestContentLength { get; init; }

public int? StatusCode { get; init; }
public string? ReasonPhrase { get; init; }
public string? ResponseHeaders { get; init; }
public string? ResponseBody { get; init; }
public string? ResponseContentType { get; init; }
public long? ResponseContentLength { get; init; }

public bool IsSuccess { get; init; }
public string? Exception { get; init; }
public string? ExceptionType { get; init; }

public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
```

}

// Event bus for decoupling
public interface IHttpLogEventBus
{
void Publish(HttpCallLogModel logModel);
IObservable<HttpCallLogModel> Subscribe();
}

public class HttpLogEventBus : IHttpLogEventBus, IDisposable
{
private readonly Subject<HttpCallLogModel> _subject;

```
public HttpLogEventBus()
{
    _subject = new Subject<HttpCallLogModel>();
}

public void Publish(HttpCallLogModel logModel)
{
    _subject.OnNext(logModel);
}

public IObservable<HttpCallLogModel> Subscribe()
{
    return _subject.AsObservable();
}

public void Dispose()
{
    _subject?.OnCompleted();
    _subject?.Dispose();
}
```

}

// Content processor
public interface IContentProcessor
{
Task<string?> ProcessAsync(HttpContent? content);
}

public class ContentProcessor : IContentProcessor
{
private static readonly string[] FileMimeTypes = {
“image/”, “video/”, “audio/”, “application/pdf”, “application/zip”,
“application/octet-stream”, “multipart/form-data”
};

```
public async Task<string?> ProcessAsync(HttpContent? content)
{
    if (content == null) return null;

    var contentType = content.Headers.ContentType?.MediaType;
    
    if (FileMimeTypes.Any(type => contentType?.StartsWith(type, StringComparison.OrdinalIgnoreCase) == true))
    {
        return $"[FILE] Type: {contentType}, Length: {content.Headers.ContentLength ?? 0}";
    }

    var body = await content.ReadAsStringAsync();
    
    if (contentType?.Contains("json", StringComparison.OrdinalIgnoreCase) == true)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch { return body; }
    }

    return body;
}
```

}

// Database repository
public interface IHttpLogRepository
{
Task SaveAsync(HttpCallLogModel logModel);
}

public class HttpLogRepository : IHttpLogRepository
{
private readonly IDbConnection _connection;

```
public HttpLogRepository(IDbConnection connection)
{
    _connection = connection;
}

public async Task SaveAsync(HttpCallLogModel logModel)
{
    const string sql = @"
        INSERT INTO HttpCallLogs (
            Id, CorrelationId, StartTime, EndTime, Duration, Method, Host, Path,
            RequestHeaders, RequestBody, RequestContentType, RequestContentLength,
            StatusCode, ReasonPhrase, ResponseHeaders, ResponseBody, ResponseContentType, ResponseContentLength,
            IsSuccess, Exception, ExceptionType, CreatedAt
        ) VALUES (
            @Id, @CorrelationId, @StartTime, @EndTime, @Duration, @Method, @Host, @Path,
            @RequestHeaders, @RequestBody, @RequestContentType, @RequestContentLength,
            @StatusCode, @ReasonPhrase, @ResponseHeaders, @ResponseBody, @ResponseContentType, @ResponseContentLength,
            @IsSuccess, @Exception, @ExceptionType, @CreatedAt
        )";

    await _connection.ExecuteAsync(sql, new
    {
        logModel.Id,
        logModel.CorrelationId,
        logModel.StartTime,
        logModel.EndTime,
        Duration = logModel.Duration?.TotalMilliseconds,
        logModel.Method,
        logModel.Host,
        logModel.Path,
        logModel.RequestHeaders,
        logModel.RequestBody,
        logModel.RequestContentType,
        logModel.RequestContentLength,
        logModel.StatusCode,
        logModel.ReasonPhrase,
        logModel.ResponseHeaders,
        logModel.ResponseBody,
        logModel.ResponseContentType,
        logModel.ResponseContentLength,
        logModel.IsSuccess,
        logModel.Exception,
        logModel.ExceptionType,
        logModel.CreatedAt
    });
}
```

}

// Reactive database logger - completely separate
public class ReactiveDbLogger : IDisposable
{
private readonly IHttpLogRepository _repository;
private readonly ILogger<ReactiveDbLogger> _logger;
private readonly IDisposable _subscription;

```
public ReactiveDbLogger(IHttpLogEventBus eventBus, IHttpLogRepository repository, ILogger<ReactiveDbLogger> logger)
{
    _repository = repository;
    _logger = logger;

    _subscription = eventBus
        .Subscribe()
        .Buffer(TimeSpan.FromSeconds(1), 50)
        .Where(buffer => buffer.Count > 0)
        .SelectMany(buffer => 
            buffer.ToObservable()
                  .Select(log => Observable.FromAsync(() => SaveLogAsync(log)))
                  .Merge(maxConcurrency: 10))
        .Subscribe(
            onNext: _ => { },
            onError: ex => _logger.LogError(ex, "Error in reactive logging pipeline")
        );
}

private async Task SaveLogAsync(HttpCallLogModel logModel)
{
    try
    {
        await _repository.SaveAsync(logModel);
        _logger.LogDebug("HTTP log saved: {CorrelationId}", logModel.CorrelationId);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to save HTTP log: {CorrelationId}", logModel.CorrelationId);
    }
}

public void Dispose()
{
    _subscription?.Dispose();
}
```

}

// HttpClient logger - only knows about event bus
public class DecoupledHttpClientLogger : IHttpClientLogger
{
private readonly ILogger<DecoupledHttpClientLogger> _logger;
private readonly IContentProcessor _contentProcessor;
private readonly IHttpLogEventBus _eventBus;
private readonly ConcurrentDictionary<string, HttpCallLogModel> _activeRequests;

```
public DecoupledHttpClientLogger(
    ILogger<DecoupledHttpClientLogger> logger,
    IContentProcessor contentProcessor,
    IHttpLogEventBus eventBus)
{
    _logger = logger;
    _contentProcessor = contentProcessor;
    _eventBus = eventBus;
    _activeRequests = new ConcurrentDictionary<string, HttpCallLogModel>();
}

public object? LogRequestStart(HttpRequestMessage request)
{
    var correlationId = Guid.NewGuid().ToString();
    
    Task.Run(async () =>
    {
        var logModel = new HttpCallLogModel
        {
            CorrelationId = correlationId,
            StartTime = DateTime.UtcNow,
            Method = request.Method.Method,
            Host = request.RequestUri?.Host ?? "",
            Path = request.RequestUri?.PathAndQuery ?? "",
            RequestHeaders = SerializeHeaders(request.Headers),
            RequestBody = await _contentProcessor.ProcessAsync(request.Content),
            RequestContentType = request.Content?.Headers.ContentType?.MediaType,
            RequestContentLength = request.Content?.Headers.ContentLength
        };

        _activeRequests[correlationId] = logModel;
        
        _logger.LogInformation("HTTP Request Started: {Method} {Host}{Path}", 
            logModel.Method, logModel.Host, logModel.Path);
    });

    return correlationId;
}

public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
{
    var correlationId = context?.ToString() ?? "";
    
    Task.Run(async () =>
    {
        if (_activeRequests.TryRemove(correlationId, out var logModel))
        {
            var completedLog = logModel with
            {
                EndTime = DateTime.UtcNow,
                Duration = elapsed,
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                ResponseHeaders = SerializeHeaders(response.Headers),
                ResponseBody = await _contentProcessor.ProcessAsync(response.Content),
                ResponseContentType = response.Content?.Headers.ContentType?.MediaType,
                ResponseContentLength = response.Content?.Headers.ContentLength,
                IsSuccess = response.IsSuccessStatusCode
            };

            _eventBus.Publish(completedLog); // Only interaction with external system
            
            _logger.LogInformation("HTTP Request Completed: {StatusCode} in {Duration}ms", 
                completedLog.StatusCode, elapsed.TotalMilliseconds);
        }
    });
}

public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed)
{
    var correlationId = context?.ToString() ?? "";
    
    Task.Run(async () =>
    {
        if (_activeRequests.TryRemove(correlationId, out var logModel))
        {
            var failedLog = logModel with
            {
                EndTime = DateTime.UtcNow,
                Duration = elapsed,
                StatusCode = response != null ? (int)response.StatusCode : null,
                ReasonPhrase = response?.ReasonPhrase,
                ResponseHeaders = response != null ? SerializeHeaders(response.Headers) : null,
                ResponseBody = response != null ? await _contentProcessor.ProcessAsync(response.Content) : null,
                ResponseContentType = response?.Content?.Headers.ContentType?.MediaType,
                ResponseContentLength = response?.Content?.Headers.ContentLength,
                IsSuccess = false,
                Exception = exception.Message,
                ExceptionType = exception.GetType().Name
            };

            _eventBus.Publish(failedLog); // Only interaction with external system
            
            _logger.LogError(exception, "HTTP Request Failed after {Duration}ms", elapsed.TotalMilliseconds);
        }
    });
}

private static string SerializeHeaders(HttpHeaders headers)
{
    return JsonSerializer.Serialize(
        headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)));
}
```

}

// Extension methods for DI registration
public static class DecoupledHttpLoggingExtensions
{
public static IServiceCollection AddDecoupledHttpClientLogging(this IServiceCollection services,
Func<IServiceProvider, IDbConnection> connectionFactory)
{
// Core components
services.AddSingleton<IContentProcessor, ContentProcessor>();
services.AddSingleton<IHttpLogEventBus, HttpLogEventBus>();
services.AddSingleton<IHttpClientLogger, DecoupledHttpClientLogger>();

```
    // Database components
    services.AddSingleton<IDbConnection>(connectionFactory);
    services.AddSingleton<IHttpLogRepository, HttpLogRepository>();
    services.AddSingleton<ReactiveDbLogger>();
    
    return services;
}
```

}

/*
Usage in Program.cs:
builder.Services.AddDecoupledHttpClientLogging(sp =>
new SqlConnection(connectionString));

The ReactiveDbLogger automatically starts listening to the event bus
when instantiated by DI container.
*/