using System.Threading;
using System.Threading.Tasks;

namespace Swapfiets.Services
{
    /// <summary>
    /// Service for getting information about bikes in different cities
    /// </summary>
    public interface IBikeService
    {
        /// <summary>
        /// Returns thefts count by city
        /// </summary>
        public Task<int> GetTheftsCountByCity(string city, int distance, CancellationToken cancellationToken);


        /// <summary>
        /// Returns thefts count by location
        /// </summary>
        public Task<int> GetTheftsCountByLocation((double lat, double lng) location, int distance, CancellationToken cancellationToken);
    }
}