using Microsoft.EntityFrameworkCore;
using QueryQuiver.Tests.Models;

namespace QueryQuiver.Tests.Mocks;
public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<PersonEntity> People => Set<PersonEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasData(PeopleMock.Generate());
            entity.OwnsOne(p => p.Address);
        });

        base.OnModelCreating(modelBuilder);
    }
}
