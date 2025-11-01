using Microsoft.EntityFrameworkCore;
using ProjectApi.Models;

namespace ProjectApi.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Kid> Kids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(255);

                entity.Property(e => e.RegistrationDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            modelBuilder.Entity<Kid>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasMaxLength(5);

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(255);

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(255);

                entity.Property(e => e.GameBalance)
                    .HasDefaultValue(0);
            });


            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Kids)
                .WithMany(k => k.Parents)
                .UsingEntity(j => j.ToTable("ParentsKids"));
        }
    }
}
