using BlogApi.Data;
using BlogApi.Services.Implementations;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
    }
}
