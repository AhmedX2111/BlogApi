using BlogApi.Models.DTOs;

namespace BlogApi.Services.Interfaces
{
	// Contract for Author-related operations
	public interface IAuthorService
	{
		Task<IEnumerable<AuthorDto>> GetAllAsync(); // Get all authors
		Task<AuthorDto?> GetByIdAsync(int id);      // Get one author by ID
		Task<AuthorDto> CreateAsync(CreateAuthorDto dto); // Create new author
		Task<AuthorDto?> UpdateAuthorAsync(int id, UpdateAuthorDto dto); // ✅ Rename here
		Task<bool> DeleteAsync(int id);             // Delete author by ID
	}
}
