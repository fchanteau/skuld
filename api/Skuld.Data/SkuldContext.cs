using Microsoft.EntityFrameworkCore;
using Skuld.Data.Entities;

#nullable disable

namespace Skuld.Data
{
	public partial class SkuldContext : DbContext
	{
		public SkuldContext ()
		{
		}

		public SkuldContext (DbContextOptions<SkuldContext> options)
			: base (options)
		{
		}

		public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer ("Server=tcp:tabletennishub.database.windows.net,1433;Initial Catalog=tabletennishub;Persist Security Info=False;User ID=CloudSA4d68c111;Password=TTyGB5S7fepJ5vZ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
			}
		}

		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation ("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

			// REFRESHTOKEN
			modelBuilder.Entity<RefreshToken> (entity =>
			{
				entity.ToTable ("RefreshToken");

				entity.HasKey (e => e.RefreshTokenId);

				entity.Property (e => e.RefreshTokenId)
					.UseIdentityColumn ();

				entity.Property (e => e.CreatedAt).HasDefaultValueSql ("(sysdatetime())");

				entity.Property (e => e.Value)
					.IsRequired ()
					.HasMaxLength (255);

				entity.HasOne (d => d.User)
					.WithMany (p => p.RefreshTokens)
					.HasForeignKey (d => d.UserId)
					.OnDelete (DeleteBehavior.Cascade)
					.HasConstraintName ("FK_RefreshToken_User");
			});

			// USER
			modelBuilder.Entity<User> (entity =>
			{
				entity.ToTable ("User");

				entity.HasKey (e => e.UserId);

				entity.Property (e => e.UserId)
					.UseIdentityColumn ();

				entity.Property (e => e.Email)
					.IsRequired ()
					.HasMaxLength (255);

				entity.Property (e => e.FirstName)
					.IsRequired ()
					.HasMaxLength (255);

				entity.Property (e => e.LastName)
					.IsRequired ()
					.HasMaxLength (255);

				entity.Property (e => e.Role)
					.HasDefaultValueSql ("((1))");
			});

			// PASSWORD
			modelBuilder.Entity<Password> (entity =>
			{
				entity.ToTable ("Password");

				entity.HasKey (e => e.PasswordId);

				entity.Property (e => e.PasswordId)
					.UseIdentityColumn ();

				entity.Property (e => e.CreatedAt).HasDefaultValueSql ("(sysdatetime())");

				entity.Property (e => e.IsActive)
					.HasColumnType ("bit")
					.HasDefaultValueSql ("((1))");

				entity.HasOne (d => d.User)
					.WithMany (p => p.Passwords)
					.HasForeignKey (d => d.UserId)
					.OnDelete (DeleteBehavior.Cascade)
					.HasConstraintName ("FK_Password_User");
			});

			OnModelCreatingPartial (modelBuilder);
		}

		partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
	}
}
