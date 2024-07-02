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

QueryQuiver is using query params of API to filter out results.
All you need to do is to pass query params to extension method `ApplyFilters()`.

### Implementation

```C#
[HttpGet]
public async Task<ActionResult<IEnumerable<Person>>> GetPeople([FromQuery] IDictionary<string, string[]> filters)
{
    return dbContext.People.ApplyFilters(filters).ToListAsync()
}
```

### Request

Data are passed as query params in request. Name of properties in request has to match model excatly to work.  
Format for filter is as follow:

`{{propertyName}}={{filterOperator}}:{{value}}`  
`rating=ge:4`

#### Spaces

You can use spaces in values (use encoding in quqery params)

#### Case sensitivy

Both property names and values are case insensitve

#### Nested/Owned properties

`job.title=eq:Miner`

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
