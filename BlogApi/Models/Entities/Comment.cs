namespace BlogApi.Models.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		// Foreign Key
		public int PostId { get; set; }
		public int AuthorId { get; set; }


		// Navigation
		public Post Post { get; set; } = default!;

		public Author Author { get; set; } = default!;
	}
}
