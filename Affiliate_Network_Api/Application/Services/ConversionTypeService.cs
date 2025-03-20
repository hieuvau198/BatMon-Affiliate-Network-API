using Application.Contracts.ConversionType;
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
    public class ConversionTypeService : IConversionTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConversionTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ConversionTypeDto>> GetAllConversionTypesAsync()
        {
            var conversionTypes = await _unitOfWork.ConversionTypes.GetAllAsync();
            var conversionTypeDtos = _mapper.Map<IEnumerable<ConversionTypeDto>>(conversionTypes);

            // Enrich DTOs with campaign count information
            foreach (var dto in conversionTypeDtos)
            {
                dto.CampaignCount = await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == dto.TypeId);
            }

            return conversionTypeDtos;
        }

        public async Task<ConversionTypeDto> GetConversionTypeByIdAsync(int id, bool includeRelated = false)
        {
            ConversionType conversionType;

            if (includeRelated)
            {
                conversionType = await _unitOfWork.ConversionTypes.GetByIdAsync(id, c => c.CampaignConversionTypes);
            }
            else
            {
                conversionType = await _unitOfWork.ConversionTypes.GetByIdAsync(id);
            }

            if (conversionType == null)
            {
                throw new KeyNotFoundException($"ConversionType with ID {id} not found");
            }

            var conversionTypeDto = _mapper.Map<ConversionTypeDto>(conversionType);

            // Add campaign count info
            conversionTypeDto.CampaignCount = await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == id);

            return conversionTypeDto;
        }

        public async Task<ConversionTypeDto> CreateConversionTypeAsync(ConversionTypeCreateDto conversionTypeDto)
        {
            // Check if name already exists (business rule: unique names)
            var existingWithSameName = await _unitOfWork.ConversionTypes.FindAsync(c => c.Name == conversionTypeDto.Name);
            if (existingWithSameName != null)
            {
                throw new InvalidOperationException($"ConversionType with name '{conversionTypeDto.Name}' already exists");
            }

            var conversionType = _mapper.Map<ConversionType>(conversionTypeDto);

            // Set default values if needed
            conversionType.RequiresApproval ??= false;

            var createdConversionType = await _unitOfWork.ConversionTypes.CreateAsync(conversionType);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ConversionTypeDto>(createdConversionType);
        }

        public async Task<ConversionTypeDto> UpdateConversionTypeAsync(ConversionTypeUpdateDto conversionTypeDto)
        {
            var existingConversionType = await _unitOfWork.ConversionTypes.GetByIdAsync(conversionTypeDto.TypeId);
            if (existingConversionType == null)
            {
                throw new KeyNotFoundException($"ConversionType with ID {conversionTypeDto.TypeId} not found");
            }

            // Check if the name is changed and the new name already exists
            if (existingConversionType.Name != conversionTypeDto.Name)
            {
                var nameExists = await _unitOfWork.ConversionTypes.ExistsAsync(
                    c => c.Name == conversionTypeDto.Name && c.TypeId != conversionTypeDto.TypeId);

                if (nameExists)
                {
                    throw new InvalidOperationException($"ConversionType with name '{conversionTypeDto.Name}' already exists");
                }
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(conversionTypeDto, existingConversionType);

            await _unitOfWork.ConversionTypes.UpdateAsync(existingConversionType);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ConversionTypeDto>(existingConversionType);

            // Add campaign count info
            result.CampaignCount = await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == conversionTypeDto.TypeId);

            return result;
        }

        public async Task DeleteConversionTypeAsync(int id)
        {
            var conversionType = await _unitOfWork.ConversionTypes.GetByIdAsync(id);
            if (conversionType == null)
            {
                throw new KeyNotFoundException($"ConversionType with ID {id} not found");
            }

            // Check if conversion type is being used by any campaigns
            var isInUse = await _unitOfWork.CampaignConversionTypes.ExistsAsync(c => c.ConversionTypeId == id);
            if (isInUse)
            {
                throw new InvalidOperationException($"Cannot delete ConversionType with ID {id} because it is being used by one or more campaigns");
            }

            await _unitOfWork.ConversionTypes.DeleteAsync(conversionType);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ConversionTypeExistsAsync(int id)
        {
            return await _unitOfWork.ConversionTypes.ExistsAsync(c => c.TypeId == id);
        }

        public async Task<int> GetConversionTypeCountAsync()
        {
            return await _unitOfWork.ConversionTypes.CountAsync(c => true);
        }

        public async Task<IEnumerable<ConversionTypeDto>> GetConversionTypesByRequiresApprovalAsync(bool requiresApproval)
        {
            var conversionTypes = await _unitOfWork.ConversionTypes.GetAllAsync(c => c.RequiresApproval == requiresApproval);
            var conversionTypeDtos = _mapper.Map<IEnumerable<ConversionTypeDto>>(conversionTypes);

            // Enrich DTOs with campaign count information
            foreach (var dto in conversionTypeDtos)
            {
                dto.CampaignCount = await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == dto.TypeId);
            }

            return conversionTypeDtos;
        }

        public async Task<IEnumerable<ConversionTypeDto>> GetConversionTypesByActionTypeAsync(string actionType)
        {
            var conversionTypes = await _unitOfWork.ConversionTypes.GetAllAsync(c => c.ActionType == actionType);
            var conversionTypeDtos = _mapper.Map<IEnumerable<ConversionTypeDto>>(conversionTypes);

            // Enrich DTOs with campaign count information
            foreach (var dto in conversionTypeDtos)
            {
                dto.CampaignCount = await _unitOfWork.CampaignConversionTypes.CountAsync(c => c.ConversionTypeId == dto.TypeId);
            }

            return conversionTypeDtos;
        }
    }
}