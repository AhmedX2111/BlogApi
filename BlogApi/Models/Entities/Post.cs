namespace BlogApi.Models.Entities
{
	public class Post
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedDate { get; set; }

		// Foreign Key
		public int AuthorId { get; set; }


		// Navigation
		public Author Author { get; set; } = default!;
		public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	}
}
