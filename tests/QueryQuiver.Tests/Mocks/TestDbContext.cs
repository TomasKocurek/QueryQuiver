using Microsoft.EntityFrameworkCore;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests.Mocks;
public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<PersonEntity> People => Set<PersonEntity>();
    public DbSet<JobEntity> Jobs => Set<JobEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var random = new Random();
        var jobs = JobMock.Generate().ToList();
        var people = PeopleMock
            .Generate()
            .Select(p =>
            {
                int randomIndex = random.Next(jobs.Count);
                p.JobId = jobs[randomIndex].Id;
                return p;
            })
            .ToList();

        modelBuilder.Entity<JobEntity>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.HasData(jobs);
        });

        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.Job).WithMany().HasForeignKey(p => p.JobId);

            entity.OwnsOne(p => p.Address).HasData(people.Select(p => AddressMock.Generate(p.Id)));
            entity.HasData(people);
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.HasData(OrderMock.Generate());
        });

        base.OnModelCreating(modelBuilder);
    }
}
