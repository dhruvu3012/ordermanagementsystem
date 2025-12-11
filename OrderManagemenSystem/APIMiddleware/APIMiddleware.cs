using Newtonsoft.Json;

namespace OrderManagemenSystem.APIMiddleware
{
    public class APIMiddleware
    {
        private readonly RequestDelegate _next;

        public APIMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;
                string jsonResponse = string.Empty;
                try
                {
                    await _next(context);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                    context.Response.Body = originalBodyStream;
                    if (context.Response.StatusCode == 200)
                    {
                        context.Response.ContentType = "application/json";
                        var response = ResponseObject.Success(responseBody, context.Response.StatusCode);
                        jsonResponse = JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        var response = ResponseObject.Error(context.Response.StatusCode, responseBody);
                        jsonResponse = JsonConvert.SerializeObject(response);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Body = originalBodyStream;
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ResponseObject.Error(500, ex.Message);
                    jsonResponse = JsonConvert.SerializeObject(errorResponse);
                }
                finally
                {
                    context.Response.Body = originalBodyStream;

                    await context.Response.WriteAsync(jsonResponse);
                }
            }

        }
    }
}
