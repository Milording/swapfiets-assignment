namespace Swapfiets.Models
{
    /// <summary>
    /// Request parameters for getting the count of thefts/>
    /// </summary>
    /// <param name="City">City name</param>
    /// <param name="Location">City location</param>
    /// <param name="Distance">Distance</param>
    public record GetTheftsCountRequest(string? City, Location? Location, int Distance = 10);

    /// <summary>
    /// Response for getting the count of thefts request
    /// </summary>
    /// <param name="Count">Total count of thefts in a city</param>
    /// <param name="Error">Error message</param>
    public record GetTheftsCountResponse(int? Count, string? Error);
}