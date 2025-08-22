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
	public class CommentServiceTests : IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly CommentService _commentService;

		public CommentServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDbContext(options);
			_commentService = new CommentService(_context);
		}

		[Fact]
		public async Task CreateAsync_Should_Add_Comment()
		{
			// Arrange
			var dto = new CreateCommentDto("Test comment", 1, 1);

			// Act
			var result = await _commentService.CreateAsync(dto);

			// Assert
			result.Id.Should().BeGreaterThan(0);
			result.Content.Should().Be("Test comment");
			result.AuthorId.Should().Be(1);
			result.PostId.Should().Be(1);
		}

		[Fact]
		public async Task GetByPostIdAsync_Should_Return_Comments()
		{
			await _commentService.CreateAsync(new CreateCommentDto("C1", 1, 1));
			await _commentService.CreateAsync(new CreateCommentDto("C2", 1, 2));

			var result = await _commentService.GetByPostIdAsync(1);

			result.Should().HaveCount(2);
		}

		[Fact]
		public async Task UpdateAsync_Should_Modify_Comment()
		{
			// Use positional constructor for UpdateCommentDto
			var created = await _commentService.CreateAsync(new CreateCommentDto("Old", 1, 1));

			var updated = await _commentService.UpdateAsync(created.Id, new UpdateCommentDto("New"));

			updated.Should().NotBeNull();
			updated!.Content.Should().Be("New");
		}

		[Fact]
		public async Task DeleteAsync_Should_Remove_Comment()
		{
			var created = await _commentService.CreateAsync(new CreateCommentDto("ToDelete", 1, 1));

			var deleted = await _commentService.DeleteAsync(created.Id);
			deleted.Should().BeTrue();

			var getAfterDelete = await _commentService.GetByPostIdAsync(1);
			getAfterDelete.Should().BeEmpty();
		}

		public void Dispose() => _context.Dispose();
	}
}
