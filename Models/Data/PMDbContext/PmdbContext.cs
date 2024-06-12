using Microsoft.EntityFrameworkCore;

namespace PM_API.Models.Data.PMDbContext;

public partial class PmdbContext : DbContext
{
    public PmdbContext()
    {
    }

    public PmdbContext(DbContextOptions<PmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Profile> Profiles { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3214EC07FAB6D726");

            entity.ToTable("Profile");

            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
