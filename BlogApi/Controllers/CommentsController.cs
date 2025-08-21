using BlogApi.Models.DTOs;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
	[ApiController] // Marks this as an API controller (automatic model validation, binding, etc.)
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")] // We’re versioning our APIs (v1 for now)
	public class CommentsController : ControllerBase
	{
		private readonly ICommentService _commentService;

		// Dependency Injection: the service is injected here
		public CommentsController(ICommentService commentService)
		{
			_commentService = commentService;
		}
		/// <summary>
		/// Get all comments for a specific post
		/// </summary>
		[HttpGet("post/{postId}")]
		public async Task<IActionResult> GetByPostId(int postId)
		{
			var comments = await _commentService.GetByPostIdAsync(postId);
			return Ok(comments); // Returns 200 with the list of comments
		}

		/// <summary>
		/// Get a single comment by Id
		/// </summary>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var comment = await _commentService.GetByPostIdAsync(id);
			if (comment == null)
				return NotFound(); // 404 if not found
			return Ok(comment);
		}

		/// <summary>
		/// Create a new comment for a post
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
		{
			var comment = await _commentService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
			// Returns 201 + location header
		}

		/// <summary>
		/// Update an existing comment
		/// </summary>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto dto)
		{
			var comment = await _commentService.UpdateAsync(id, dto);
			if (comment == null)
				return NotFound();
			return Ok(comment);
		}

		/// <summary>
		/// Delete a comment by Id
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _commentService.DeleteAsync(id);
			if (!deleted)
				return NotFound();
			return NoContent(); // 204
		}
	}
}
