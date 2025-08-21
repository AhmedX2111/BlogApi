using BlogApi.Models.DTOs;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private readonly IAuthorService _authorService;

		// Inject the AuthorService via constructor
		public AuthorsController(IAuthorService authorService)
		{
			_authorService = authorService;
		}
		// GET: api/authors
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors()
		{
			var authors = await _authorService.GetAllAsync();
			return Ok(authors);
		}

		// GET: api/authors/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<AuthorDto>> GetAuthorById(int id)
		{
			var author = await _authorService.GetByIdAsync(id);
			if (author == null) return NotFound(); // 404 if not found
			return Ok(author);
		}

		// POST: api/authors
		[HttpPost]
		public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto dto)
		{
			var newAuthor = await _authorService.CreateAsync(dto);
			// Return 201 Created + location of the new resource
			return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor);
		}

		// PUT: api/authors/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, UpdateAuthorDto dto)
		{
			var updated = await _authorService.UpdateAuthorAsync(id, dto);
			if (updated == null) return NotFound(); // if author doesn’t exist
			return Ok(updated);
		}

		// DELETE: api/authors/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAuthor(int id)
		{
			var deleted = await _authorService.DeleteAsync(id);
			if (!deleted) return NotFound();
			return NoContent(); // 204
		}
	}
}
