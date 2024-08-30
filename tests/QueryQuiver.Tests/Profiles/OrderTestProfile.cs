using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests.Profiles;
internal class OrderTestProfile : MapProfile<OrderDto, OrderEntity>
{
    public OrderTestProfile()
    {
        AddProperty(nameof(OrderDto.OrderDateTime), nameof(OrderEntity.DateTime));
        AddProperty(nameof(OrderDto.OrderPrice), nameof(OrderEntity.Price));
    }
}
