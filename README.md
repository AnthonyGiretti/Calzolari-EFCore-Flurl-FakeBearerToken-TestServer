# Clean integration tests with Calzolari.TestServer.EntityFramework 

A fluent library to easily write integration tests for ASP.NET Core with EntityFramework Core.

Following example uses AutoFixture and xUnit but you can use other tools if you desire.

**Parallelism MUST BE deactivated while testing**, the database is created and removed after each test which avoids any data collision, the DbContext is reinstancianted between each test.

It's not recommended for large databases.

This library uses [Flurl](https://flurl.dev/) and [FakeBearerToken](https://github.com/webmotions/fake-authentication-jwtbearer) ([Dominique St-Amand](https://github.com/DOMZE))

# Steps to setup your project

## Create your integration test project

Optionally you can add:

[xUnit](https://xunit.net/) as tests runner

[FluentAssertions.Web](https://github.com/adrianiftode/FluentAssertions.Web) for assertions on HttpResponseMessage

[AutoFixture](https://github.com/AutoFixture/AutoFixture) for autofill any type of object

## Install the package Calzolari.TestServer.EntityFramework

The package can be found here: https://www.nuget.org/packages/Calzolari.TestServer.EntityFramework/

The version 5.0.5 is the current version (.NET 5), a .NET Core 3.1 is coming soon

## Create a dedicated Startup file for your integration tests

Add the method **AddApplicationPart** which takes in parameter any class of your webapplication, this is needed to retrieve controllers and register them in the integration test project like this:

```csharp
services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
```

Then register The DbContext with the following method: **AddIntegrationTestDbContext**, this is needed for integration test to be working with EF Core, behind the scene it register your DbContext with SQLite Database:

```csharp
services.AddIntegrationTestDbContext<DemoDbContext>()
```

Finally register the fake bearer token authentication like this:

```csharp
services.AddFakeBearerToken();
```

This is needed to create an identity with a fake token during the tests

## Create a Web factory

Inherit from FlurlWebFactory<T>, generic T can be any class of your webapplication, then override **CreateHostBuilder** and use your integration Startup file like this:
 
```csharp
using Calzolari.TestServer.EntityFramework.Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Calzolari.WebApi.Tests.Common
{
    public class DemoFactory : FlurlWebFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHost((builder) =>
            {
                builder.UseStartup<TestStartup>();
            });
        }
    }
}
```

## Create a xUnit collection definition

A collection definition allows injection dependency (Singleton lifetime) within your test classes. The following example shows how to register a collection named "AssemblyFixture" that allows to pass into your test classes the webfactory named "DemoFactory":

```csharp
using Xunit;

namespace Calzolari.WebApi.Tests.Common
{
    [CollectionDefinition("AssemblyFixture")]
    public class AssemblyFixture : ICollectionFixture<DemoFactory> { }
}
```

## Create a base test class

Inherit from IntegrationTestBase<TDbContext, T>, TDbContext is your DbContext and T is the same class that you defined as generic parameter on FlurlWebFactory<T>
Add [Collection("AssemblyFixture")] attribute on your base test class. This is here where you can add AutoFixture or anything else that you need for your test.
Your base test class should look like this:
 
```csharp
using AutoFixture;
using Calzolari.TestServer.EntityFramework.Database.EF;
using Calzolari.WebApi.Database;
using Xunit;

namespace Calzolari.WebApi.Tests.Common
{
    [Collection("AssemblyFixture")]
    public class DemoTestBase : IntegrationTestBase<DemoDbContext, Startup>
    {
        protected readonly IFixture Fixture;

        public DemoTestBase(DemoFactory factory) : base(factory)
        {
            Fixture = new Fixture();
        }
    }
}
```

## Write your tests

Create a test class and inherit from the base test class. Pass the web factory by injection dependency and write your tests.
I suggest to use [FluentAssertions](https://fluentassertions.com/) for your assertions

To feed the database use the method Arrange like this:

```csharp
var countries = Fixture.Build<Country>()
                       .CreateMany(3);
  
Arrange(dbContext =>
{
    dbContext.Countries.AddRange(countries);
});
```

If you expect an auto incremented Id it works like if you were using EntityFramework within your application:

```csharp
var country = Fixture.Create<Country>();

Arrange(dbContext => { dbContext.Countries.Add(country); });

// country.CountryId CountryId is filled
```
Then call your System Under Test (SUT):

```csharp
var response = await BASE_REQUEST.Route(BaseRoute).GetAsync();
```

And assert the result:

```csharp
response.ResponseMessage
	.Should()
	.Be200Ok()
	.And
	.BeAs(countries);
```                    
                    
Simple usage of GET verb can be found here: https://github.com/AnthonyGiretti/Calzolari-EFCore-Flurl-FakeBearerToken-TestServer/blob/main/Calzolari.WebApi.Tests/CountryControllerTests/GetByIdTests.cs

Simple usage of POST verb can be found here with the usage of the fake bearer token: https://github.com/AnthonyGiretti/Calzolari-EFCore-Flurl-FakeBearerToken-TestServer/blob/main/Calzolari.WebApi.Tests/CountryControllerTests/PostTests.cs
