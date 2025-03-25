using Application.Contracts.CampaignConversionType;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CampaignConversionTypeService : ICampaignConversionTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignConversionTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignConversionTypeDto>> GetAllCampaignConversionTypesAsync()
        {
            var campaignConversionTypes = await _unitOfWork.CampaignConversionTypes.GetAllAsync(
                null, // predicate parameter
                x => x.Campaign!,
                x => x.ConversionType!
            );

            return _mapper.Map<IEnumerable<CampaignConversionTypeDto>>(campaignConversionTypes);
        }

        public async Task<CampaignConversionTypeDto> GetCampaignConversionTypeByIdAsync(int id)
        {
            var campaignConversionType = await _unitOfWork.CampaignConversionTypes.GetByIdAsync(
                id,
                x => x.Campaign!,
                x => x.ConversionType!);

            if (campaignConversionType == null)
                throw new KeyNotFoundException($"CampaignConversionType with ID {id} not found.");

            return _mapper.Map<CampaignConversionTypeDto>(campaignConversionType);
        }

        public async Task<IEnumerable<CampaignConversionTypeDto>> GetCampaignConversionTypesByCampaignIdAsync(int campaignId)
        {
            var campaignConversionTypes = await _unitOfWork.CampaignConversionTypes.GetAllAsync(
                x => x.CampaignId == campaignId,
                x => x.Campaign!,
                x => x.ConversionType!);

            return _mapper.Map<IEnumerable<CampaignConversionTypeDto>>(campaignConversionTypes);
        }

        public async Task<IEnumerable<CampaignConversionTypeDto>> GetCampaignConversionTypesByConversionTypeIdAsync(int conversionTypeId)
        {
            var campaignConversionTypes = await _unitOfWork.CampaignConversionTypes.GetAllAsync(
                x => x.ConversionTypeId == conversionTypeId,
                x => x.Campaign!,
                x => x.ConversionType!);

            return _mapper.Map<IEnumerable<CampaignConversionTypeDto>>(campaignConversionTypes);
        }

        public async Task<CampaignConversionTypeDto> CreateCampaignConversionTypeAsync(CampaignConversionTypeCreateDto campaignConversionTypeDto)
        {
            // Check if campaign exists
            var campaignExists = await _unitOfWork.Campaigns.ExistsAsync(c => c.CampaignId == campaignConversionTypeDto.CampaignId);
            if (!campaignExists)
                throw new KeyNotFoundException($"Campaign with ID {campaignConversionTypeDto.CampaignId} not found.");

            // Check if conversion type exists
            var conversionTypeExists = await _unitOfWork.ConversionTypes.ExistsAsync(c => c.TypeId == campaignConversionTypeDto.ConversionTypeId);
            if (!conversionTypeExists)
                throw new KeyNotFoundException($"ConversionType with ID {campaignConversionTypeDto.ConversionTypeId} not found.");

            // Check if mapping already exists
            var mappingExists = await _unitOfWork.CampaignConversionTypes.ExistsAsync(
                c => c.CampaignId == campaignConversionTypeDto.CampaignId &&
                     c.ConversionTypeId == campaignConversionTypeDto.ConversionTypeId);

            if (mappingExists)
                throw new InvalidOperationException($"A mapping between Campaign {campaignConversionTypeDto.CampaignId} and ConversionType {campaignConversionTypeDto.ConversionTypeId} already exists.");

            var campaignConversionType = _mapper.Map<CampaignConversionType>(campaignConversionTypeDto);

            var createdEntity = await _unitOfWork.CampaignConversionTypes.CreateAsync(campaignConversionType);
            await _unitOfWork.SaveChangesAsync();

            return await GetCampaignConversionTypeByIdAsync(createdEntity.CampaignConversionId);
        }

        public async Task<CampaignConversionTypeDto> UpdateCampaignConversionTypeAsync(CampaignConversionTypeUpdateDto campaignConversionTypeDto)
        {
            var existingEntity = await _unitOfWork.CampaignConversionTypes.GetByIdAsync(campaignConversionTypeDto.CampaignConversionId);

            if (existingEntity == null)
                throw new KeyNotFoundException($"CampaignConversionType with ID {campaignConversionTypeDto.CampaignConversionId} not found.");

            _mapper.Map(campaignConversionTypeDto, existingEntity);

            await _unitOfWork.CampaignConversionTypes.UpdateAsync(existingEntity);
            await _unitOfWork.SaveChangesAsync();

            return await GetCampaignConversionTypeByIdAsync(existingEntity.CampaignConversionId);
        }

        public async Task<bool> DeleteCampaignConversionTypeAsync(int id)
        {
            var campaignConversionType = await _unitOfWork.CampaignConversionTypes.GetByIdAsync(id);

            if (campaignConversionType == null)
                return false;

            // Check if there are any conversions using this mapping
            var hasConversions = await _unitOfWork.Conversions.ExistsAsync(c => c.CampaignConversionTypeId == id);

            if (hasConversions)
                throw new InvalidOperationException($"Cannot delete CampaignConversionType with ID {id} because it has associated conversions.");

            await _unitOfWork.CampaignConversionTypes.DeleteAsync(campaignConversionType);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CampaignConversionTypeExistsAsync(int id)
        {
            return await _unitOfWork.CampaignConversionTypes.ExistsAsync(c => c.CampaignConversionId == id);
        }

        public async Task<int> GetCountByCampaignIdAsync(int campaignId)
        {
            return await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.CampaignId == campaignId);
        }

        public async Task<int> GetCountByConversionTypeIdAsync(int conversionTypeId)
        {
            return await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == conversionTypeId);
        }
    }
}