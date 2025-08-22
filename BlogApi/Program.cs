using BlogApi.Data;
using BlogApi.Middleware;
using BlogApi.Services.Implementations;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace BlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);
			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddApiVersioning(options =>
			{
				options.AssumeDefaultVersionWhenUnspecified = true;         // Default if version not specified
				options.DefaultApiVersion = new ApiVersion(1, 0);           // Default = v1.0
				options.ReportApiVersions = true;                           // Returns version info in headers
			});
			builder.Services.AddRateLimiter(options =>
			{
				options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
				{
					// Use client IP as key
					var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

					return RateLimitPartition.GetFixedWindowLimiter(ip, key =>
						new FixedWindowRateLimiterOptions
						{
							PermitLimit = 100,
							Window = TimeSpan.FromMinutes(1),
							QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
							QueueLimit = 50
						});
				});

				options.RejectionStatusCode = 429;
			});

			builder.Services.AddScoped<IAuthorService, AuthorService>();
			builder.Services.AddScoped<IPostService, PostService>();
			builder.Services.AddScoped<ICommentService, CommentService>();


			// ✅ Register ApplicationDbContext with SQL Server
			// Reads connection string from appsettings.json → "DefaultConnection"
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<ErrorHandlingMiddleware>();

			app.UseRateLimiter();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
    }
}
