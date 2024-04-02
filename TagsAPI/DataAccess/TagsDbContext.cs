using Microsoft.EntityFrameworkCore;
using TagsAPI.Domain;
using SL = TagsAPI.DataAccess.Consts.StringLengths;

namespace TagsAPI.DataAccess
{
    public class TagsDbContext(DbContextOptions<TagsDbContext> options) : DbContext(options)
    {
        public DbSet<Tag> Tags => Set<Tag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTags(modelBuilder);
        }

        private static void ConfigureTags(ModelBuilder builder)
        {
            builder.Entity<Tag>(cfg =>
            {
                cfg.HasKey(e => e.Id);

                cfg.Property(e => e.Name)
                    .HasMaxLength(SL.TinyString);
                cfg.HasIndex(e => e.Name).IsUnique();

                cfg.Property(e => e.Count)
                    .IsRequired()
                    .HasDefaultValue(0);
                cfg.ToTable(t => t.HasCheckConstraint("CK_Tag_Count", "Count >= 0"));

                cfg.Property(e => e.Share)
                    .IsRequired()
                    .HasDefaultValue(0)
                    .HasPrecision(18, 2);
                cfg.ToTable(t => t.HasCheckConstraint("CK_Tag_Share", "Share >= 0 AND Share <= 1"));
            });
        }
    }
}
