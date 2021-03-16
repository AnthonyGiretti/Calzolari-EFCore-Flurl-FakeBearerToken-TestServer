using Xunit;

namespace Calzolari.WebApi.Tests.Common
{
    [CollectionDefinition("AssemblyFixture")]
    public class AssemblyFixture : ICollectionFixture<DemoFactory> { }
}