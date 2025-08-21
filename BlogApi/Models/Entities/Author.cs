namespace BlogApi.Models.Entities
{
	public class Author
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string? Bio { get; set; }
		public DateTime JoinDate { get; set; } = DateTime.UtcNow;

		// Navigation
		public ICollection<Post> Posts { get; set; } = new List<Post>();
		public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	}
}
