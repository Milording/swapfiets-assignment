using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Swapfiets.Services.Models;

namespace Swapfiets.Services
{
    public class BikeService : IBikeService
    {
        private const string Bikeindex = "bikeindex";

        private readonly IHttpClientFactory httpClientFactory;


        public BikeService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<int> GetTheftsCountByCity(string city, int distance, CancellationToken cancellationToken)
        {
            var httpClient = this.httpClientFactory.CreateClient(Bikeindex);

            var response = await httpClient.GetFromJsonAsync<SearchCountResponse>(
                               httpClient.BaseAddress + $"/api/v3/search/count?location={city}&distance={distance}&stolenness=proximity",
                               cancellationToken);

            return response?.Proximity ?? 0;
        }


        public async Task<int> GetTheftsCountByLocation((double lat, double lng) location, int distance, CancellationToken cancellationToken)
        {
            var (lat, lng) = location;

            var httpClient = this.httpClientFactory.CreateClient(Bikeindex);

            var response = await httpClient.GetFromJsonAsync<SearchCountResponse>(
                               httpClient.BaseAddress + $"/api/v3/search/count?location={lat},{lng}&distance={distance}&stolenness=proximity",
                               cancellationToken);

            return response?.Proximity ?? 0;
        }
    }
}