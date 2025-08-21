using BlogApi.Models.DTOs;

namespace BlogApi.Services.Interfaces
{
	// Contract for Post-related operations
	public interface IPostService
	{
		// Get all posts
		Task<IEnumerable<PostDto>> GetAllPostsAsync();

		// Get a single post by its Id
		Task<PostDto?> GetPostByIdAsync(int id);

		// Create a new post
		Task<PostDto> CreatePostAsync(CreatePostDto dto);

		// Update an existing post
		Task<PostDto?> UpdatePostAsync(int id, UpdatePostDto dto);

		// Delete a post by Id
		Task<bool> DeletePostAsync(int id);
	}
}
