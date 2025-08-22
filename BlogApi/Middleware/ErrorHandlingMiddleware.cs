using System.Net;
using System.Text.Json;

namespace BlogApi.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception occurred.");
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = ex switch
				{
					KeyNotFoundException => (int)HttpStatusCode.NotFound,
					ArgumentException => (int)HttpStatusCode.BadRequest,
					_ => (int)HttpStatusCode.InternalServerError
				};

				var response = new { message = ex.Message };
				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
	}	}
}
