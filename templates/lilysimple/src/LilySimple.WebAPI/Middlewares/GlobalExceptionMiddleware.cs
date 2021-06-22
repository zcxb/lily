using LilySimple.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger,
                                         RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ERROR LOG] {ErrorOccurredAt}\n" +
                    "        TraceId: {traceId}\n" +
                    "        Endpoint: {method} {endpoint}\n",
                    DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    context.TraceIdentifier,
                    context.Request.Method,
                    context.Request.Path);
                var response = new Flag
                {
                    Success = false,
                    Msg = "An unknown error occurred.",
                };

                if (ex is BizException bex)
                {
                    response.Msg = bex.Message;
                }

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
            }
        }
    }
}
