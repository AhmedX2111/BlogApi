namespace BlogApi.Models.DTOs;

public record PostDto(int Id, string Title, string Content, DateTime CreatedDate, DateTime? UpdatedDate, int AuthorId);
public record CreatePostDto(string Title, string Content, int AuthorId);
public record UpdatePostDto(string Title, string Content);