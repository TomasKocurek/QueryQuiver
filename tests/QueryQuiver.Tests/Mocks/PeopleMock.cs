﻿using Bogus;
using PersonModel = QueryQuiver.Tests.Models.PersonEntity;

namespace QueryQuiver.Tests.Mocks;
internal static class PeopleMock
{
    public static List<PersonModel> Generate(int count = 100)
    {
        return new Faker<PersonModel>()
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Age, f => f.Random.Number(18, 99))
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.GDPR, f => f.Random.Bool())
            .Generate(count);
    }
}
