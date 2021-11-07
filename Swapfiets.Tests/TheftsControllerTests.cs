using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Swapfiets.Controllers;
using Swapfiets.Models;
using Swapfiets.Services;
using Xunit;

namespace Swapfiets.Tests
{
    public class TheftsControllerTests
    {
        [Fact]
        public async Task CityExists_ReturnsCountOfThefts()
        {
            // arrange
            var bikeServiceMock = new Mock<IBikeService>();
            bikeServiceMock
                .Setup(s => s.GetTheftsCountByCity(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            var request = new GetTheftsCountRequest("Amsterdam", Location: null, Distance: 10);

            // act
            var response = await new TheftsController(bikeServiceMock.Object).GetTheftsCount(request, CancellationToken.None);

            // assert
            response.Should().BeEquivalentTo(new GetTheftsCountResponse(10, null));
        }


        [Fact]
        public async Task LocationExists_ReturnsCountOfThefts()
        {
            // arrange
            var bikeServiceMock = new Mock<IBikeService>();
            bikeServiceMock
                .Setup(s => s.GetTheftsCountByLocation(It.IsAny<(double lat, double lng)>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            var request = new GetTheftsCountRequest(City: null, new Location(52.3676, 4.9041), Distance: 10);

            // act
            var response = await new TheftsController(bikeServiceMock.Object).GetTheftsCount(request, CancellationToken.None);

            // assert
            response.Should().BeEquivalentTo(new GetTheftsCountResponse(10, null));
        }


        [Fact]
        public async Task CityAndLocationProvided_ReturnsError()
        {
            // arrange
            var bikeServiceMock = new Mock<IBikeService>();
            var request = new GetTheftsCountRequest("Amsterdam", new Location(52.3676, 4.9041), Distance: 10);

            // act
            var response = await new TheftsController(bikeServiceMock.Object).GetTheftsCount(request, CancellationToken.None);

            // assert
            response.Should().BeEquivalentTo(
                new GetTheftsCountResponse(0,
                                           "Please check the city is correct or increase the distance by passing it as a parameter (default is 10 miles). " +
                                           "Also you can't pass the city and location at the same time"));
        }


        [Fact]
        public async Task ThrowsHttpException_ReturnsError()
        {
            // arrange
            var bikeServiceMock = new Mock<IBikeService>();
            bikeServiceMock
                .Setup(s => s.GetTheftsCountByLocation(It.IsAny<(double lat, double lng)>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException("Oops, problem"));
            var request = new GetTheftsCountRequest(City: null, new Location(52.3676, 4.9041), Distance: 10);

            // act
            var response = await new TheftsController(bikeServiceMock.Object).GetTheftsCount(request, CancellationToken.None);

            // assert
            response.Should().BeEquivalentTo(
                new GetTheftsCountResponse(0,
                                           "An error occurred when receiving the data: Oops, problem"));
        }
    }
}