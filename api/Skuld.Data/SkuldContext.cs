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

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation ("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            // REFRESHTOKEN
            modelBuilder.Entity<RefreshToken> (entity =>
            {
                entity.ToTable ("RefreshToken");

                entity.HasKey (e => e.RefreshTokenId);

                entity.Property (e => e.RefreshTokenId)
                    .HasColumnType ("numeric(18, 0)")
                    .UseIdentityColumn ();

                entity.Property (e => e.CreatedDate).HasDefaultValueSql ("(sysdatetime())");

                entity.Property (e => e.Value)
                    .IsRequired ()
                    .HasMaxLength (255);

                entity.Property (e => e.UserId).HasColumnType ("numeric(18, 0)");

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
                    .HasColumnType ("numeric(18, 0)")
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


                entity.Property (e => e.Role).HasDefaultValueSql ("((1))");
            });

            OnModelCreatingPartial (modelBuilder);
        }

        partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
    }
}
