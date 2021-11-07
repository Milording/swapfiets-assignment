using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swapfiets.Models;
using Swapfiets.Services;

namespace Swapfiets.Controllers
{
    [ApiController]
    public class TheftsController : ControllerBase
    {
        private readonly IBikeService bikeService;


        public TheftsController(IBikeService bikeService)
        {
            this.bikeService = bikeService;
        }


        [HttpGet("/thefts/")]
        public async Task<GetTheftsCountResponse> GetTheftsCount([FromQuery] GetTheftsCountRequest parameters, CancellationToken cancellationToken)
        {
            int theftsCount = 0;

            try
            {
                theftsCount = parameters switch
                {
                    ({ }, null, _) { City: { } } request => await this.bikeService.GetTheftsCountByCity(request.City, request.Distance, cancellationToken),
                    (null, { }, _) { Location: { } } request => await this.bikeService.GetTheftsCountByLocation(
                                                                    (request.Location.Lat, request.Location.Lng),
                                                                    request.Distance,
                                                                    cancellationToken),
                    _ => 0
                };
            }
            catch (HttpRequestException e)
            {
                return new GetTheftsCountResponse(Count: 0, Error: $"An error occurred when receiving the data: {e.Message}");
            }

            var additionalMessage = theftsCount == 0
                                        ? "Please check the city is correct or increase the distance by passing it as a parameter (default is 10 miles). " +
                                          "Also you can't pass the city and location at the same time"
                                        : null;

            return new GetTheftsCountResponse(theftsCount, additionalMessage);
        }
    }
}