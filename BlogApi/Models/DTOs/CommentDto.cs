namespace BlogApi.Models.DTOs;

public record CommentDto(int Id, string Content, DateTime CreatedDate, int PostId, int AuthorId);
public record CreateCommentDto(string Content, int PostId, int AuthorId);
public record UpdateCommentDto(string Content);
