using ePizza.Core.CustomExceptions;
using ePizza.Models.Response;
using Serilog.Context;
using System.Security.Authentication;
using System.Text.Json;

namespace ePizza.API.Middlewares
{
    public class CommonResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CommonResponseMiddleware> _logger;

        public CommonResponseMiddleware(
            RequestDelegate next,
            ILogger<CommonResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId =  context.TraceIdentifier;
            LogContext.PushProperty("RequestId", requestId);

            var originalBodyStream = context.Response.Body;

            using(var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);

                    if (context.Response.ContentType != null
                        && context.Response.ContentType.Contains("application/json"))  // only process if response is in json format
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);   // start reading memory stream

                        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync(); // read the response

                        // create response object
                        var repsonseObj
                            = new ApiResponseModel<object>(
                                  success: context.Response.StatusCode >= 200 && context.Response.StatusCode <= 300,
                                  data: JsonSerializer.Deserialize<object>(responseBody)!,
                                  message: "Request completed successfully"
                                );

                        var jsonResponse = JsonSerializer.Serialize(repsonseObj);
                        context.Response.Body = originalBodyStream;
                        await context.Response.WriteAsync(jsonResponse); // send respone back to user
                    }


                }
                catch(InvalidCredentialException ex)
                {
                    context.Response.StatusCode = 401;
                    var errorResponse = new
                    {
                        success = false,
                        data = (object)null,
                        message = ex.Message
                    };
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    context.Response.Body = originalBodyStream;

                    _logger.LogError($"Invalid Credentials passed: {ex.Message}");
                    await context.Response.WriteAsync(jsonResponse);
                }
                catch (RecordNotFoundException ex)
                {
                    context.Response.StatusCode = 400;
                    var errorResponse = new
                    {
                        success = false,
                        data = (object)null,
                        message = ex.Message
                    };
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    context.Response.Body = originalBodyStream;
                    _logger.LogError($"Record Not Found: {ex.Message}");
                    await context.Response.WriteAsync(jsonResponse);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    var errorResponse = new
                    {
                        success = false,
                        data = (object)null,
                        message = ex.Message
                    };
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    context.Response.Body = originalBodyStream;
                    _logger.LogError($"Unhandled Exception: {ex.Message}");
                    await context.Response.WriteAsync(jsonResponse);
                }
            }

          
        }
    }
}
