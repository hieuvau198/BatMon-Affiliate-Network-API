using Application.Contracts.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();
        Task<PublisherDto> GetPublisherByIdAsync(int id, bool includeRelated = false);
        Task<PublisherDto> GetPublisherByEmailAsync(string email);
        Task<PublisherDto> GetPublisherByUsernameAsync(string username);
        Task<PublisherDto> CreatePublisherAsync(PublisherCreateDto publisherDto);
        Task<PublisherDto> UpdatePublisherAsync(PublisherUpdateDto publisherDto);
        Task DeletePublisherAsync(int id);
        Task<bool> ActivatePublisherAsync(int id);
        Task<bool> DeactivatePublisherAsync(int id);
        Task<bool> PublisherExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<int> GetPublisherCountAsync();
        Task<IEnumerable<PublisherDto>> GetPublishersByReferrerCodeAsync(string referralCode);
        Task<int> GetReferralCountAsync(string referralCode);
    }
}
