using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LilySimple.Middlewares
{
    public class RequestLogMiddleware
    {
        private readonly ILogger<RequestLogMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(ILogger<RequestLogMiddleware> logger,
                                    RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            #region 基础变量
            string requestContent = string.Empty; //请求信息，响应信息
            StreamReader requestReader = null;

            StringBuilder logTemplate = new StringBuilder(); //日志模板
            List<object> logParams = new List<object>();// 日志中的各个参数

            #endregion

            #region 记录请求信息
            logTemplate.Append("[REQUEST LOG] {RequestBeginAt}\n");
            logParams.Add(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            logTemplate.Append("        TraceId {traceId}\n");
            logParams.Add(context.TraceIdentifier);
            logTemplate.Append("        Uri: {Method} {RequestPath}{QueryString}\n");
            logParams.Add(context.Request.Method);
            logParams.Add(context.Request.Path);
            logParams.Add(context.Request.QueryString);
            var headers = context.Request.Headers;
            if (headers.Any())
            {
                if (context.Request.Headers.ContainsKey(HeaderNames.Authorization))
                {
                    try
                    {
                        string authorization = context.Request.Headers[HeaderNames.Authorization];
                        var token = authorization.Substring("Bearer ".Length).Trim();
                        var handler = new JwtSecurityTokenHandler();
                        var payload = handler.ReadJwtToken(token).Payload;
                        logTemplate.Append("        JwtBearerPayload: {JwtBearerPayload}\n");
                        logParams.Add(payload);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }

            }
            #endregion
            #region 获取请求信息
            if (context.Request.ContentType != null && context.Request.ContentType.Contains(MediaTypeNames.Application.Json))
            {
                context.Request.EnableBuffering();//必须设置，否则无法读取
                requestReader = new StreamReader(context.Request.Body, Encoding.UTF8);
                requestContent = await requestReader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                logTemplate.Append("        Req: {RequestBody}\n");
                logParams.Add(requestContent);
            }
            #endregion
            _logger.LogInformation(logTemplate.ToString(), logParams.ToArray());

            await _next.Invoke(context);//执行后续中间件
        }
    }
}
