# QueryQuiver

Simple way to add filtering, pagination and sorting to you API.

## Instalation

You can instal QueryQuiver via NuGet

```
Install-Package QueryQuiver
```

Or via .NET CLI

```
dotnet add package QueryQuiver
```

## Usage

QueryQuiver is using query params of API to filter out results. IFilteringService is used to apply filters to IQueryable.
Each method has two types params. First is Dto model, which API is returning and second is Database Entity model, over which filters are applied.
Method takes two arguments. First is IQueryable of database entity and second is dictionary of query params.

### Example

```C#
[ApiController]
[Route("[controller]")]
public class OrdersController(IFilteringService FilteringService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DataList<OrderEntity>>> GetOrders([FromQuery] IDictionary<string, string[]> filters)
    {
        var result = await FilteringService.ExecuteAsync<OrderDto, OrderEntity>(dbContext.Orders, filters);
        return Ok(result)
    }
}
```

### IFilteringService

The service is returning DataList with filtered results. Service is returning database entity, which has to be mapped to DTO model.
Service supports both sync and async methods. 
Types parametrs of methods are used to retrive mapping profile (if exists)

### Dependency Injection

To use IFilteringService you have to register in via AddQueryQuiver() method. 
If you want to use mapping profiles you have to register via AddMappingProfiles() and pass assembly where profiles are located.

```C#

### Request

Data are passed as query params in request. Those property names should match property names/paths in your dto model.
If DTO does not match your database entity model you can use MappingProfile to map between properties.
Format for filter is as follow:

`{{propertyName}}={{filterOperator}}:{{value}}`  
`rating=ge:4`

#### Spaces

You can use spaces in values (use encoding in quqery params)

#### Case sensitivy

Both property names and values are case insensitve

#### Nested/Owned properties

For nested properties use `|` to separate path to property

`job|title=eq:Miner`

#### Pagination

Pagination requires two parameters  
`page` (offset of results) - default 0  
`pageSize` (number of results) - default 20  
If parametr is not support default values are used

#### Sort

Currently only one sorting parametr is allowed  
`{{sort}}={{propertyName}}:{{desc/asc}}`

#### Filter operators

| Operation          | Query |
| ------------------ | ----- |
| Equal              | eq    |
| NotEqual           | ne    |
| GreaterThan        | gt    |
| GreaterThanOrEqual | ge    |
| LessThan           | lt    |
| LessThanOrEqual    | le    |
| Contains           | ct    |
| StartsWith         | sw    |
| EndsWith           | ew    |

### Mapping
For cases when DTO and Entity model do not match you can use MappingProfile to map between properties. 
Mapping only works for translation of filters to database entity. IFiltering service returns database entity which has to be mapped
Library is expecting that consumer of API is using properties names from DTO.
To create map you have to create new class which will inherit from MappingProfile and in constructor you can use MapProperty method to map properties.
If properties names/paths are identical you do not have to create mapping profile or map these concrete properties.

```C#
internal class OrderTestProfile : MappingProfile<OrderDto, OrderEntity>
{
    public OrderTestProfile()
    {
        MapProperty(dto => dto.OrderDateTime, entity => entity.DateTime);
        MapProperty(dto => dto.OrderPrice, entity => entity.Price);
        MapProperty(dto => dto.CustomerEmail, entity => entity.Customer.Email);
    }
}
```