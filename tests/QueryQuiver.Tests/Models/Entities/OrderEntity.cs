namespace QueryQuiver.Tests.Models.Entities;
public class OrderEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime DateTime { get; set; }
    public decimal Price { get; set; }
}
