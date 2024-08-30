namespace QueryQuiver.Tests.Models.Dtos;
internal record OrderDto
{
    public DateTime OrderDateTime { get; init; }
    public decimal OrderPrice { get; init; }
}
