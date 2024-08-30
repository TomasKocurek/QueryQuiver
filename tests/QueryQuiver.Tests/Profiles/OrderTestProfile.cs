using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests.Profiles;
internal class OrderTestProfile : MappingProfile<OrderDto, OrderEntity>
{
    public OrderTestProfile()
    {
        MapProperty(dto => dto.OrderDateTime, entity => entity.DateTime);
        MapProperty(dto => dto.OrderPrice, entity => entity.Price);
        MapProperty(dto => dto.CustomerEmail, entity => entity.Customer.Email);
    }
}
