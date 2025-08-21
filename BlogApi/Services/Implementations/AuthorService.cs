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
			return await _context.Authors
				.Select(a => new AuthorDto(a.Id, a.Name, a.Email, a.Bio, a.JoinDate))
				.ToListAsync();
		}

		public async Task<AuthorDto?> GetByIdAsync(int id)
		{
			var author = await _context.Authors.FindAsync(id);
			if (author == null) return null;


			return new AuthorDto(author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
		}

		public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
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

			return new AuthorDto (author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
		}

		public async Task<AuthorDto?> UpdateAuthorAsync(int id, UpdateAuthorDto dto)
		{
			var author = await _context.Authors.FindAsync(id);
			if (author == null) return null;

			author.Name = dto.Name;
			author.Email = dto.Email;
			author.Bio = dto.Bio;

			await _context.SaveChangesAsync();

			return new AuthorDto(author.Id, author.Name, author.Email, author.Bio, author.JoinDate);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var author = await _context.Authors.FindAsync(id);
			if (author == null) return false;

			_context.Authors.Remove(author);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
