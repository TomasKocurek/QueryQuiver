using Bogus;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests.Mocks;
internal static class OrderMock
{
    public static List<OrderEntity> Generate(int count = 25)
        => new Faker<OrderEntity>()
            .RuleFor(p => p.DateTime, f => f.Date.Past())
            .RuleFor(p => p.Price, f => f.Finance.Amount())
            .Generate(count);
}
