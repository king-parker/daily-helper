using Microsoft.EntityFrameworkCore;

namespace DailyHelperApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Models.WorkoutEntry> Workouts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.WorkoutEntry>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Exercise)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Weight)
                    .IsRequired();

                entity.Property(e => e.Sets)
                    .IsRequired();

                entity.Property(e => e.Reps)
                    .IsRequired();

                entity.Property(e => e.Date)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);
            });
        }
    }
}
