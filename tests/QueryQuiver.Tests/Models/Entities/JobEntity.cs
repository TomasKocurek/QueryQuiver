namespace QueryQuiver.Tests.Models.Entities;
public class JobEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public long Salary { get; set; }
}
