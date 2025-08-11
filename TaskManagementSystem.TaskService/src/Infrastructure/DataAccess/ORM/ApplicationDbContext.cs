using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.TaskService.Core.Aggregates;

namespace TaskManagementSystem.TaskService.Infrastructure.DataAccess.ORM;


public class ApplicationDbContext : DbContext
{
    public DbSet<TaskAggregate> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskAggregate>(builder => ConfigureTask(builder));
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}

    private void ConfigureTask(EntityTypeBuilder<TaskAggregate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new {
            x.BoardId,
            x.ColumnId
        });

        builder.HasIndex(x => new {
                x.Rank,
                x.BoardId
            })
            .IsUnique();

        builder.HasIndex(x => x.BoardId);
        builder.HasIndex(x => x.ColumnId);
        builder.HasIndex(x => x.Rank);

        builder.OwnsOne(e => e.Timestamps, t =>
        {
            t.Property(p => p.CreatedAt)
                .HasColumnName("created_at");
            t.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at");
        });
        builder.OwnsOne(e => e.AuthorInfo, a =>
        {
            a.Property(p => p.CreatedById)
                .HasColumnName("created_by_id");
            a.Property(p => p.UpdatedById)
                .HasColumnName("updated_by_id");
        });
    }
}
