using Bogus;
using QueryQuiver.Tests.Models;

namespace QueryQuiver.Tests.Mocks;
internal static class JobMock
{
    public static List<JobEntity> Generate(int count = 25)
    {
        return new Faker<JobEntity>()
            .RuleFor(p => p.Title, f => f.Name.JobTitle())
            .RuleFor(p => p.Salary, f => f.Random.Number(1000, 100000))
            .Generate(count);
    }
}
