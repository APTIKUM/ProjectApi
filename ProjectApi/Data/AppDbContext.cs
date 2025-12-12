using Microsoft.EntityFrameworkCore;
using ProjectApi.Models;

namespace ProjectApi.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Kid> Kids { get; set; }

        public DbSet<KidTask> KidTasks { get; set; }

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

            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Kids)
                .WithMany(k => k.Parents)
                .UsingEntity(j => j.ToTable("ParentsKids"));

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

            

            modelBuilder.Entity<KidTask>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Price)
                    .IsRequired();

                entity.Property(e => e.TimeStart)
                    .IsRequired();

                entity.Property(e => e.TimeEnd);

                entity.Property(e => e.RepeatDaysJson)
                    .HasDefaultValue("[]");

                entity.Property(e => e.CompletedDatesJson)
                    .HasDefaultValue("[]");

                entity.Property(e => e.ImageUrl)
                   .HasDefaultValue("")
                   .HasMaxLength(255);

                entity.HasOne(kt => kt.Kid)
                    .WithMany(k => k.Tasks)
                    .HasForeignKey(kt => kt.KidId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}
