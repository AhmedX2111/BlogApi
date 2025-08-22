using BlogApi.Data;
using BlogApi.Models.DTOs;
using BlogApi.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Tests.Services
{
	public class AuthorServiceTests : IDisposable
	{
		private readonly AuthorService _authorService;
		private readonly ApplicationDbContext _context;

		public AuthorServiceTests()
		{
			// Use EF InMemory for testing with a unique database name per test instance
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDbContext(options);
			_authorService = new AuthorService(_context);
		}

		[Fact]
		public async Task AddAuthor_Should_Add_New_Author()
		{
			// Arrange
			var dto = new CreateAuthorDto("John Doe", "john@example.com", "Writer");

			// Act
			var result = await _authorService.CreateAsync(dto);

			// Assert
			result.Id.Should().BeGreaterThan(0);
			result.Name.Should().Be("John Doe");
			result.Email.Should().Be("john@example.com");
		}

		[Fact]
		public async Task GetAuthors_Should_Return_All_Authors()
		{
			// Arrange
			await _authorService.CreateAsync(new CreateAuthorDto("Test", "t@t.com", "Bio"));

			// Act
			var result = await _authorService.GetAllAsync();

			// Assert
			result.Should().NotBeEmpty();
			result.Should().HaveCountGreaterThan(0);
		}

		[Fact]
		public async Task GetAuthor_Should_Return_Correct_Author()
		{
			// Arrange
			var created = await _authorService.CreateAsync(new CreateAuthorDto("Jane", "jane@test.com", "Bio"));

			// Act
			var result = await _authorService.GetByIdAsync(created.Id);

			// Assert
			result.Should().NotBeNull();
			result!.Name.Should().Be("Jane");
			result.Email.Should().Be("jane@test.com");
		}

		[Fact]
		public async Task UpdateAuthor_Should_Modify_Author_Details()
		{
			// Arrange
			var created = await _authorService.CreateAsync(new CreateAuthorDto("Old Name", "old@test.com", "Old Bio"));
			var updateDto = new UpdateAuthorDto("New Name", "new@test.com", "New Bio");

			// Act
			var updated = await _authorService.UpdateAuthorAsync(created.Id, updateDto);

			// Assert
			updated.Should().NotBeNull();
			updated!.Name.Should().Be("New Name");
			updated.Email.Should().Be("new@test.com");
		}

		[Fact]
		public async Task DeleteAuthor_Should_Remove_Author()
		{
			// Arrange
			var created = await _authorService.CreateAsync(new CreateAuthorDto("Delete Me", "delete@test.com", "Bio"));

			// Act
			var result = await _authorService.DeleteAsync(created.Id);

			// Assert
			result.Should().BeTrue();

			var deleted = await _authorService.GetByIdAsync(created.Id);
			deleted.Should().BeNull();
		}

		public void Dispose()
		{
			_context?.Dispose();
		}

	}
}
