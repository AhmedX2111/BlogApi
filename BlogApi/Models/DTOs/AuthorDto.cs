namespace BlogApi.Models.DTOs;

public record AuthorDto(int Id, string Name, string Email, string? Bio, DateTime JoinDate);
public record CreateAuthorDto(string Name, string Email, string? Bio);
public record UpdateAuthorDto(string Name, string Email, string? Bio);