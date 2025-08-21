using BlogApi.Data;
using BlogApi.Models.DTOs;
using BlogApi.Models.Entities;
using BlogApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Implementations
{
	public class PostService : IPostService
	{
		private readonly ApplicationDbContext _context;

		public PostService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
		{
			return await _context.Posts
				.Select(p => new PostDto(
					p.Id,
					p.Title,
					p.Content,
					p.CreatedDate,
					p.UpdatedDate,
					p.AuthorId
				))
				.ToListAsync();
		}

		public async Task<PostDto?> GetPostByIdAsync(int id)
		{
			var post = await _context.Posts.FindAsync(id);

			return post == null
				? null
				: new PostDto(post.Id, post.Title, post.Content, post.CreatedDate, post.UpdatedDate, post.AuthorId);
		}

		public async Task<PostDto> CreatePostAsync(CreatePostDto dto)
		{
			var post = new Post
			{
				Title = dto.Title,
				Content = dto.Content,
				AuthorId = dto.AuthorId,
				CreatedDate = DateTime.UtcNow
			};

			_context.Posts.Add(post);
			await _context.SaveChangesAsync();

			return new PostDto(post.Id, post.Title, post.Content, post.CreatedDate, post.UpdatedDate, post.AuthorId);
		}

		public async Task<PostDto?> UpdatePostAsync(int id, UpdatePostDto dto)
		{
			var post = await _context.Posts.FindAsync(id);
			if (post == null) return null;

			post.Title = dto.Title;
			post.Content = dto.Content;
			post.UpdatedDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return new PostDto(post.Id, post.Title, post.Content, post.CreatedDate, post.UpdatedDate, post.AuthorId);
		}

		public async Task<bool> DeletePostAsync(int id)
		{
			var post = await _context.Posts.FindAsync(id);
			if (post == null) return false;

			_context.Posts.Remove(post);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
