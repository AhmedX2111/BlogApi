📖 BlogApi – ASP.NET Core 8 Web API

A blogging API built with ASP.NET Core 8, Entity Framework Core 8, Serilog, and Swagger.
Supports CRUD for Authors, Posts, and Comments with sample DTOs and service layer abstraction.

🚀 Features

ASP.NET Core 8 Web API with layered architecture (Controllers → Services → Data).

Entity Framework Core 8 with SQL Server.

DTOs for clean API contracts.

Swagger/OpenAPI for testing endpoints.

Serilog for logging.

API Versioning (applied in CommentsController only as api/v1.0/comments).

⚙️ Setup Instructions
1. Clone the Repository
git clone https://github.com/yourusername/BlogApi.git
cd BlogApi/BlogApi

2. Configure Database

Update appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=BlogApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

3. Apply Migrations & Create Database
dotnet ef migrations add InitialCreate
dotnet ef database update

4. Run the Application
dotnet run


By default, Swagger UI will be available at:

https://localhost:5001/swagger

📚 API Documentation
🔹 Authors Endpoints (/api/Authors)

GET /api/Authors → Get all authors.

GET /api/Authors/{id} → Get author by ID.

POST /api/Authors → Create a new author.

{
  "name": "John Doe",
  "email": "johndoe@example.com",
  "bio": "Book writer"
}


PUT /api/Authors/{id} → Update author.

DELETE /api/Authors/{id} → Delete author.

🔹 Posts Endpoints (/api/Posts)

GET /api/Posts → Get all posts.

GET /api/Posts/{id} → Get post by ID.

POST /api/Posts → Create new post.

{
  "title": "My first post",
  "content": "Hello blog!",
  "authorId": 1
}


PUT /api/Posts/{id} → Update post.

DELETE /api/Posts/{id} → Delete post.

🔹 Comments Endpoints (Versioned – /api/v1.0/Comments)

⚠️ Note: Only CommentsController uses API versioning.

GET /api/v1.0/Comments/post/{postId} → Get all comments for a post.

GET /api/v1.0/Comments/{id} → Get comment by ID.

POST /api/v1.0/Comments → Create new comment.

{
  "content": "Nice post!",
  "postId": 1,
  "authorId": 1
}


PUT /api/v1.0/Comments/{id} → Update comment.

DELETE /api/v1.0/Comments/{id} → Delete comment.

🛠️ Tech Stack

.NET 8

Entity Framework Core 8 (Code First)

SQL Server

Serilog (logging)

Swagger (Swashbuckle.AspNetCore)

API Versioning (only in CommentsController)
