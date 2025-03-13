using Application.Contracts.TrafficSource;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrafficSourceService
    {
        Task<IEnumerable<TrafficSourceDto>> GetAllTrafficSourcesAsync();
        Task<TrafficSourceDto> GetTrafficSourceByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<TrafficSourceDto>> GetTrafficSourcesByPublisherIdAsync(int publisherId);
        Task<TrafficSourceDto> CreateTrafficSourceAsync(TrafficSourceCreateDto trafficSourceDto);
        Task<TrafficSourceDto> UpdateTrafficSourceAsync(TrafficSourceUpdateDto trafficSourceDto);
        Task DeleteTrafficSourceAsync(int id);
        Task<bool> ActivateTrafficSourceAsync(int id);
        Task<bool> DeactivateTrafficSourceAsync(int id);
        Task<bool> TrafficSourceExistsAsync(int id);
        Task<int> GetTrafficSourceCountAsync();
        Task<int> GetTrafficSourceCountByPublisherAsync(int publisherId);
        Task<IEnumerable<TrafficSourceDto>> GetActiveTrafficSourcesAsync();
        Task<IEnumerable<TrafficSourceDto>> GetTrafficSourcesByTypeAsync(string type);
    }
}