using System.Diagnostics;

namespace TaskForge.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public CorrelationIdMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }


        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            context.Items["CorrelationId"] = correlationId;

            context.Response.Headers["X-Correlation-Id"] = correlationId;
        
            using(Activity activity = new Activity("Http Request") )
            {
                activity.SetIdFormat(ActivityIdFormat.W3C);
                activity.Start();

                activity.SetTag("correlation_id", correlationId);

                await _requestDelegate(context);
            }
        }
    }
}
