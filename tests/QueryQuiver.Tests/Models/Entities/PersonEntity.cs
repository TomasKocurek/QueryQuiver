using QueryQuiver.Tests.Models.Enums;

namespace QueryQuiver.Tests.Models.Entities;
public class PersonEntity
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public bool GDPR { get; set; }
    public JobEntity Job { get; set; } = null!;
    public string JobId { get; set; } = null!;
    public Status Status { get; set; }
}
