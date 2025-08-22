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
	public class PostServiceTests : IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly PostService _postService;

		public PostServiceTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDbContext(options);
			_postService = new PostService(_context);
		}

		[Fact]
		public async Task CreatePostAsync_Should_Add_Post()
		{
			// Arrange
			var dto = new CreatePostDto("Test Post", "This is a test post", 1);

			// Act
			var result = await _postService.CreatePostAsync(dto);

			// Assert
			result.Id.Should().BeGreaterThan(0);
			result.Title.Should().Be("Test Post");
			result.Content.Should().Be("This is a test post");
			result.AuthorId.Should().Be(1);
			result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
			result.UpdatedDate.Should().BeNull();
		}

		[Fact]
		public async Task GetAllPostsAsync_Should_Return_Posts()
		{
			await _postService.CreatePostAsync(new CreatePostDto("P1", "C1", 1));
			await _postService.CreatePostAsync(new CreatePostDto("P2", "C2", 1));

			var result = await _postService.GetAllPostsAsync();

			result.Should().HaveCount(2);
		}

		[Fact]
		public async Task GetPostByIdAsync_Should_Return_Correct_Post()
		{
			var created = await _postService.CreatePostAsync(new CreatePostDto("Title", "Content", 1));

			var result = await _postService.GetPostByIdAsync(created.Id);

			result.Should().NotBeNull();
			result!.Title.Should().Be("Title");
			result.Content.Should().Be("Content");
			result.AuthorId.Should().Be(1);
		}

		[Fact]
		public async Task UpdatePostAsync_Should_Modify_Post()
		{
			var created = await _postService.CreatePostAsync(new CreatePostDto("Old", "Old Content", 1));

			var updated = await _postService.UpdatePostAsync(created.Id, new UpdatePostDto("New", "New Content"));

			updated.Should().NotBeNull();
			updated!.Title.Should().Be("New");
			updated.Content.Should().Be("New Content");
			updated.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
		}

		[Fact]
		public async Task DeletePostAsync_Should_Remove_Post()
		{
			var created = await _postService.CreatePostAsync(new CreatePostDto("Delete", "X", 1));

			var deleted = await _postService.DeletePostAsync(created.Id);
			deleted.Should().BeTrue();

			var getAfterDelete = await _postService.GetAllPostsAsync();
			getAfterDelete.Should().BeEmpty();
		}

		public void Dispose() => _context.Dispose();
	}
}
