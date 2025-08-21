using BlogApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Author> Authors => Set<Author>();
		public DbSet<Post> Posts => Set<Post>();
		public DbSet<Comment> Comments => Set<Comment>();
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Relationships
			// Author → Posts (Cascade OK)
			modelBuilder.Entity<Post>()
				.HasOne(p => p.Author)
				.WithMany(a => a.Posts)
				.HasForeignKey(p => p.AuthorId)
				.OnDelete(DeleteBehavior.Cascade);

			// Post → Comments (Cascade OK)
			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			// Author → Comments (❌ Restrict cascade to avoid multiple paths)
			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Author)
				.WithMany(a => a.Comments)
				.HasForeignKey(c => c.AuthorId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
