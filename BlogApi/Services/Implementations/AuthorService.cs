using BlogApi.Data;
using BlogApi.Models.DTOs;
using BlogApi.Models.Entities;
using BlogApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Implementations
{
	// Implements author business logic
	public class AuthorService : IAuthorService
	{
		private readonly ApplicationDbContext _context;
		public AuthorService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<AuthorDto>> GetAllAsync()
		{
			try
			{
				return await _context.Authors
					.Select(a => new AuthorDto(a.Id, a.Name, a.Email, a.Bio, a.JoinDate))
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Error retrieving authors", ex);
			}
		}

		public async Task<AuthorDto?> GetByIdAsync(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);
				if (author == null) return null;
				return new AuthorDto(author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving author with id {id}", ex);
			}
		}

		public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
		{
			try
			{
				var author = new Author
				{
					Name = dto.Name,
					Email = dto.Email,
					Bio = dto.Bio,
					JoinDate = DateTime.UtcNow
				};
				_context.Authors.Add(author);
				await _context.SaveChangesAsync();
				return new AuthorDto(author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating author", ex);
			}
		}

		public async Task<AuthorDto?> UpdateAuthorAsync(int id, UpdateAuthorDto dto)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);
				if (author == null) return null;

				author.Name = dto.Name;
				author.Email = dto.Email;
				author.Bio = dto.Bio;

				await _context.SaveChangesAsync();
				return new AuthorDto(author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error updating author with id {id}", ex);
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);
				if (author == null) return false;

				_context.Authors.Remove(author);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error deleting author with id {id}", ex);
			}
	}	}
}
