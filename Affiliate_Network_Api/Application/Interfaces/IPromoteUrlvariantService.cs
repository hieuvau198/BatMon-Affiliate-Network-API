using Application.Contracts.PromoteUrlvariantService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPromoteUrlvariantService
    {
        Task<IEnumerable<PromoteUrlvariantDto>> GetAllPromoteUrlvariantsAsync();
        Task<PromoteUrlvariantDto> GetPromoteUrlvariantByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<PromoteUrlvariantDto>> GetPromoteUrlvariantsByPromoteIdAsync(int promoteId);
        Task<IEnumerable<PromoteUrlvariantDto>> GetPromoteUrlvariantsByTrafficSourceIdAsync(int trafficSourceId);
        Task<PromoteUrlvariantDto> CreatePromoteUrlvariantAsync(PromoteUrlvariantCreateDto promoteUrlvariantDto);
        Task<PromoteUrlvariantDto> UpdatePromoteUrlvariantAsync(PromoteUrlvariantUpdateDto promoteUrlvariantDto);
        Task DeletePromoteUrlvariantAsync(int id);
        Task<bool> ActivatePromoteUrlvariantAsync(int id);
        Task<bool> DeactivatePromoteUrlvariantAsync(int id);
        Task<bool> PromoteUrlvariantExistsAsync(int id);
        Task<int> GetPromoteUrlvariantCountAsync();
        Task<int> GetPromoteUrlvariantCountByPromoteAsync(int promoteId);
        Task<int> GetPromoteUrlvariantCountByTrafficSourceAsync(int trafficSourceId);
        Task<IEnumerable<PromoteUrlvariantDto>> GetActivePromoteUrlvariantsAsync();
    }
}
