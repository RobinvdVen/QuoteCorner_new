namespace Domain.Common.Interfaces
{
    public interface IGeoLocationService
    {
        Task<string> GetLocationFromIpAddress(string ipAddress);
    }
}
