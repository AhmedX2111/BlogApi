using BlogApi.Models.DTOs;

namespace BlogApi.Services.Interfaces
{
	// Contract for Comment-related operations
	public interface ICommentService
	{
		Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId);
		Task<CommentDto> CreateAsync(CreateCommentDto dto);
		Task<CommentDto?> UpdateAsync(int id, UpdateCommentDto dto);
		Task<bool> DeleteAsync(int id);
	}
}
