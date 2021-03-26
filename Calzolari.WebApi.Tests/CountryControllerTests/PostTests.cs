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
    public class PostTests : DemoTestBase
    {
        private const string BaseRoute = "/api/country";

        public PostTests(DemoFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task When_route_is_correct_and_authorization_well_setup_should_return_Created()
        {
            // Arrange
            var country = Fixture.Build<Country>().Without(c => c.CountryId).Create();
            var token = new
            {
                sub = "Anthony Giretti",
                role = new[] { "Admin" }
            };

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).FakeToken(token).PostJsonAsync(country);

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be201Created()
                    .And.BeAs(new
                    {
                        country.CountryName,
                        country.Description
                    });
        }

        [Fact]
        public async Task When_route_is_correct_and_role_missing_should_return_Forbidden()
        {
            // Arrange
            var country = Fixture.Create<Country>();
            var token = new
            {
                sub = "Anthony Giretti"
            };

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).FakeToken(token).PostJsonAsync(country);

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be403Forbidden();
        }

        [Fact]
        public async Task When_route_is_correct_and_role_is_wrong_should_return_Forbidden()
        {
            // Arrange
            var country = Fixture.Create<Country>();
            var token = new
            {
                sub = "Anthony Giretti",
                role = new[] { "WrongRole" }
            };

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).FakeToken(token).PostJsonAsync(country);

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be403Forbidden();
        }

        [Fact]
        public async Task When_route_is_correct_and_token_is_missing_should_return_Forbidden()
        {
            // Arrange
            var country = Fixture.Create<Country>();

            // Act
            var response = await BASE_REQUEST.Route(BaseRoute).PostJsonAsync(country);

            // Assert
            response.ResponseMessage
                    .Should()
                    .Be401Unauthorized();
        }
    }
}