using LoggingApi.Database;

namespace LoggingApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext appDbContext)
        {
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            appDbContext.Logs.Add(new LogModel
            {
                Request = requestBody,
                Response = responseBodyText,
                Timestamp = DateTime.UtcNow
            });

            await appDbContext.SaveChangesAsync();
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

}
