// Application/Interfaces/IPublisherBalanceService.cs
using Application.Contracts.PublisherBalance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPublisherBalanceService
    {
        Task<IEnumerable<PublisherBalanceDto>> GetAllPublisherBalancesAsync();
        Task<PublisherBalanceDto> GetPublisherBalanceByIdAsync(int id);
        Task<PublisherBalanceDto> GetPublisherBalanceByPublisherIdAsync(int publisherId);
        Task<PublisherBalanceDto> CreatePublisherBalanceAsync(PublisherBalanceCreateDto publisherBalanceDto);
        Task<PublisherBalanceDto> UpdatePublisherBalanceAsync(PublisherBalanceUpdateDto publisherBalanceDto);
        Task DeletePublisherBalanceAsync(int id);
        Task<bool> PublisherBalanceExistsAsync(int id);
    }
}