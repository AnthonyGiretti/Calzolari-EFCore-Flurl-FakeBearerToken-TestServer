using System.Collections.Generic;
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
    public class GetTests : DemoTestBase
    {
        private const string BaseRoute = "/api/country";

        public GetTests(DemoFactory factory) : base(factory) { }

        [Fact]
        public async Task When_route_is_correct_and_database_fed_should_return_Ok_and_collection_of_country()
        {
            // Arrange
            var countries = Fixture.Build<Country>()
                                   .CreateMany(3);

            Arrange(dbContext =>
            {
                dbContext.Countries.AddRange(countries);
            });

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).GetAsync();

            // Assert
            response.ResponseMessage
                .Should()
                .Be200Ok()
                .And
                .BeAs(countries);
        }

        [Fact]
        public async Task When_route_is_not_correct_should_return_NotFound()
        {
            // Arrange

            // Act
            var response = await BASE_REQUEST.Route("wrongroute").GetAsync();

            // Assert
            response.ResponseMessage.Should().Be404NotFound();
        }

        [Fact]
        public async Task When_route_is_correct_and_database_not_fed_should_return_Ok_and_empty_collection_of_country()
        {
            // Arrange

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).GetAsync();

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be200Ok()
                    .And
                    .Satisfy<IEnumerable<Country>>(model =>
                    {
                        model.Should().BeNullOrEmpty();
                    });
        }
    }
}