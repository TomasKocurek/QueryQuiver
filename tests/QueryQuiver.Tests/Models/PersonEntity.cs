namespace QueryQuiver.Tests.Models;
public class PersonEntity
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public bool IsEmployed { get; set; }
}
