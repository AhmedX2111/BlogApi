using BlogApi.Models.DTOs;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly IPostService _postService;

		// Inject the PostService
		public PostsController(IPostService postService)
		{
			_postService = postService;
		}
		// GET: api/posts
		[HttpGet]
		public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts()
		{
			var posts = await _postService.GetAllPostsAsync();
			return Ok(posts);
		}

		// GET: api/posts/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<PostDto>> GetPostById(int id)
		{
			var post = await _postService.GetPostByIdAsync(id);
			if (post == null) return NotFound();
			return Ok(post);
		}

		// POST: api/posts
		[HttpPost]
		public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto dto)
		{
			var newPost = await _postService.CreatePostAsync(dto);
			return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
		}

		// PUT: api/posts/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult<PostDto>> UpdatePost(int id, UpdatePostDto dto)
		{
			var updated = await _postService.UpdatePostAsync(id, dto);
			if (updated == null) return NotFound();
			return Ok(updated);
		}

		// DELETE: api/posts/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var deleted = await _postService.DeletePostAsync(id);
			if (!deleted) return NotFound();
			return NoContent();
		}
	}
}
