using Application.Contracts.FraudReport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFraudReportService
    {
        Task<IEnumerable<FraudReportDto>> GetAllFraudReportsAsync();
        Task<FraudReportDto> GetFraudReportByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<FraudReportDto>> GetFraudReportsByCampaignIdAsync(int campaignId);
        Task<IEnumerable<FraudReportDto>> GetFraudReportsByPublisherIdAsync(int publisherId);
        Task<IEnumerable<FraudReportDto>> GetFraudReportsByAdvertiserIdAsync(int advertiserId);
        Task<IEnumerable<FraudReportDto>> GetFraudReportsByStatusAsync(string status);
        Task<IEnumerable<FraudReportDto>> GetUnreadFraudReportsAsync();
        Task<FraudReportDto> CreateFraudReportAsync(FraudReportCreateDto fraudReportDto);
        Task<FraudReportDto> UpdateFraudReportAsync(FraudReportUpdateDto fraudReportDto);
        Task DeleteFraudReportAsync(int id);
        Task<bool> MarkFraudReportAsReadAsync(int id);
        Task<bool> MarkFraudReportAsUnreadAsync(int id);
        Task<bool> FraudReportExistsAsync(int id);
        Task<int> GetFraudReportCountAsync();
        Task<int> GetUnreadFraudReportCountAsync();
        Task<decimal> GetTotalFinancialImpactAsync(int? advertiserId = null, int? publisherId = null);
    }
}