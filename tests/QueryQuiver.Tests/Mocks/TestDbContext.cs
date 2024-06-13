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
        var people = PeopleMock.Generate();

        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.OwnsOne(p => p.Address).HasData(people.Select(p => AddressMock.Generate(p.Id)));
            entity.HasData(people);
        });

        base.OnModelCreating(modelBuilder);
    }
}
