using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace MangaBackend.Middleware.MiddlewareClasses;
public class ResponseTimeMiddleware : IMiddleware
{
    // Name of the Response Header, Custom Headers starts with "X-"  
    private const string RESPONSE_HEADER_RESPONSE_TIME = "X-Response-Time-ms";
    // Handle to the next Middleware in the pipeline

    private readonly ILogger<ResponseTimeMiddleware> _logger;

    public ResponseTimeMiddleware(ILogger<ResponseTimeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("HAAAAAAAi");

        // Start the Timer using Stopwatch  
        //var watch = new Stopwatch();
        //watch.Start();
        //context.Response.OnStarting(() => {
        //    // Stop the timer information and calculate the time   
        //    watch.Stop();
        //    var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
        //    // Add the Response time information in the Response headers.   
        //    context.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = responseTimeForCompleteRequest.ToString();

        //    return Task.CompletedTask;
        //});

        // Call the next delegate/middleware in the pipeline   
        await next.Invoke(context);
    }
}

