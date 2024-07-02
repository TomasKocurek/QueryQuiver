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

        base.OnModelCreating(modelBuilder);
    }
}
