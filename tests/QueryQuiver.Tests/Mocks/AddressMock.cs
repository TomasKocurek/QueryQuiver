using Bogus;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests.Mocks;
internal static class AddressMock
{
    public static Address Generate(string personId)
    {
        var address = new Faker<Address>()
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.Number, f => f.Address.BuildingNumber())
            .RuleFor(a => a.Country, f => f.Address.Country())
            .Generate();
        address.PersonEntityId = personId;
        return address;
    }
}
