namespace Swapfiets.Services.Models
{
    /// <summary>
    /// Response with count of bike thefts
    /// </summary>
    /// <param name="Proximity">Number of thefts</param>
    public record SearchCountResponse(int Proximity);
}