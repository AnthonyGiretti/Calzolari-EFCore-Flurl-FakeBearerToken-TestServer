using System.Threading.Tasks;
using AutoFixture;
using Calzolari.TestServer.EntityFramework.Flurl;
using Calzolari.WebApi.Models;
using Calzolari.WebApi.Tests.Common;
using FluentAssertions;
using Flurl.Http;
using Xunit;

namespace Calzolari.WebApi.Tests.CountryControllerTests
{
    public class GetByIdTests : DemoTestBase
    {
        private const string BaseRoute = "/api/country/{0}";

        public GetByIdTests(DemoFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task When_route_is_correct_and_database_fed_should_return_Ok_and_the_expected_country()
        {
            // Arrange
            var country = Fixture.Create<Country>();

            Arrange(dbContext => { dbContext.Countries.Add(country); });

            // Act
            var response = await BASE_REQUEST.Route(string.Format(BaseRoute, country.CountryId)).GetAsync();

            // Assert
            response.ResponseMessage
                .Should()
                .Be200Ok()
                .And
                .BeAs(country);
        }


        [Fact]
        public async Task When_route_is_not_correct_and_database_fed_should_return_NotFound()
        {
            // Arrange
            var country = Fixture.Create<Country>();
            var wrongCountryId = Fixture.Create<int>();

            Arrange(dbContext => { dbContext.Countries.Add(country); });

            // Act
            var response = await BASE_REQUEST.Route(string.Format(BaseRoute, wrongCountryId)).GetAsync();

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be404NotFound();
        }
    }
}