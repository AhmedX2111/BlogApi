using BlogApi.Data;
using BlogApi.Models.DTOs;
using BlogApi.Models.Entities;
using BlogApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Implementations
{
	public class CommentService : ICommentService
	{
		private readonly ApplicationDbContext _context;

		public CommentService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId)
		{
			try
			{
				return await _context.Comments
					.Where(c => c.PostId == postId)
					.Select(c => new CommentDto(c.Id, c.Content, c.CreatedDate, c.AuthorId, c.PostId))
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving comments for post id {postId}", ex);
			}
		}

		public async Task<CommentDto> CreateAsync(CreateCommentDto dto)
		{
			try
			{
				var comment = new Comment
				{
					Content = dto.Content,
					CreatedDate = DateTime.UtcNow,
					AuthorId = dto.AuthorId,
					PostId = dto.PostId
				};

				_context.Comments.Add(comment);
				await _context.SaveChangesAsync();

				return new CommentDto(comment.Id, comment.Content, comment.CreatedDate, comment.AuthorId, comment.PostId);
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating comment", ex);
			}
		}

		public async Task<CommentDto?> UpdateAsync(int id, UpdateCommentDto dto)
		{
			try
			{
				var comment = await _context.Comments.FindAsync(id);
				if (comment == null) return null;

				comment.Content = dto.Content;
				await _context.SaveChangesAsync();

				return new CommentDto(comment.Id, comment.Content, comment.CreatedDate, comment.AuthorId, comment.PostId);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error updating comment with id {id}", ex);
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				var comment = await _context.Comments.FindAsync(id);
				if (comment == null) return false;

				_context.Comments.Remove(comment);
				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error deleting comment with id {id}", ex);
			}
		}
	}
}
