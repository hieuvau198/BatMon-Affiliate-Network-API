using Application.Contracts.PromoteUrlvariantService;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PromoteUrlvariantService : IPromoteUrlvariantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromoteUrlvariantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PromoteUrlvariantDto>> GetAllPromoteUrlvariantsAsync()
        {
            var variants = await _unitOfWork.PromoteUrlvariants.GetAllAsync(
                null,
                p => p.Promote,
                p => p.TrafficSource
            );
            return _mapper.Map<IEnumerable<PromoteUrlvariantDto>>(variants);
        }

        public async Task<PromoteUrlvariantDto> GetPromoteUrlvariantByIdAsync(int id, bool includeRelated = false)
        {
            PromoteUrlvariant? variant;

            if (includeRelated)
            {
                variant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(
                    id,
                    p => p.Promote,
                    p => p.TrafficSource
                );
            }
            else
            {
                variant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(id);
            }

            if (variant == null)
            {
                throw new KeyNotFoundException($"PromoteUrlvariant with ID {id} not found");
            }

            return _mapper.Map<PromoteUrlvariantDto>(variant);
        }

        public async Task<IEnumerable<PromoteUrlvariantDto>> GetPromoteUrlvariantsByPromoteIdAsync(int promoteId)
        {
            var promote = await _unitOfWork.Promotes.GetByIdAsync(promoteId);
            if (promote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {promoteId} not found");
            }

            var variants = await _unitOfWork.PromoteUrlvariants.GetAllAsync(
                p => p.PromoteId == promoteId,
                p => p.TrafficSource
            );

            return _mapper.Map<IEnumerable<PromoteUrlvariantDto>>(variants);
        }

        public async Task<IEnumerable<PromoteUrlvariantDto>> GetPromoteUrlvariantsByTrafficSourceIdAsync(int trafficSourceId)
        {
            var trafficSource = await _unitOfWork.TrafficSources.GetByIdAsync(trafficSourceId);
            if (trafficSource == null)
            {
                throw new KeyNotFoundException($"TrafficSource with ID {trafficSourceId} not found");
            }

            var variants = await _unitOfWork.PromoteUrlvariants.GetAllAsync(
                p => p.TrafficSourceId == trafficSourceId,
                p => p.Promote
            );

            return _mapper.Map<IEnumerable<PromoteUrlvariantDto>>(variants);
        }

        public async Task<PromoteUrlvariantDto> CreatePromoteUrlvariantAsync(PromoteUrlvariantCreateDto variantDto)
        {
            // Validate promote exists if ID is provided
            if (variantDto.PromoteId.HasValue && !await _unitOfWork.Promotes.ExistsAsync(p => p.PromoteId == variantDto.PromoteId))
            {
                throw new KeyNotFoundException($"Promote with ID {variantDto.PromoteId} not found");
            }

            // Validate traffic source exists if ID is provided
            if (variantDto.TrafficSourceId.HasValue && !await _unitOfWork.TrafficSources.ExistsAsync(t => t.SourceId == variantDto.TrafficSourceId))
            {
                throw new KeyNotFoundException($"TrafficSource with ID {variantDto.TrafficSourceId} not found");
            }

            // Validate URLs if provided
            if (!string.IsNullOrWhiteSpace(variantDto.CustomUrl) && !Uri.IsWellFormedUriString(variantDto.CustomUrl, UriKind.Absolute))
            {
                throw new ArgumentException("The provided Custom URL is not valid");
            }

            var variant = _mapper.Map<PromoteUrlvariant>(variantDto);

            // Set default values
            variant.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            variant.IsActive = variantDto.IsActive ?? true;

            var createdVariant = await _unitOfWork.PromoteUrlvariants.CreateAsync(variant);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PromoteUrlvariantDto>(createdVariant);
        }

        public async Task<PromoteUrlvariantDto> UpdatePromoteUrlvariantAsync(PromoteUrlvariantUpdateDto variantDto)
        {
            var existingVariant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(variantDto.VariantId);
            if (existingVariant == null)
            {
                throw new KeyNotFoundException($"PromoteUrlvariant with ID {variantDto.VariantId} not found");
            }

            // Validate promote exists if ID is provided
            if (variantDto.PromoteId.HasValue && !await _unitOfWork.Promotes.ExistsAsync(p => p.PromoteId == variantDto.PromoteId))
            {
                throw new KeyNotFoundException($"Promote with ID {variantDto.PromoteId} not found");
            }

            // Validate traffic source exists if ID is provided
            if (variantDto.TrafficSourceId.HasValue && !await _unitOfWork.TrafficSources.ExistsAsync(t => t.SourceId == variantDto.TrafficSourceId))
            {
                throw new KeyNotFoundException($"TrafficSource with ID {variantDto.TrafficSourceId} not found");
            }

            // Validate URLs if provided
            if (!string.IsNullOrWhiteSpace(variantDto.CustomUrl) && !Uri.IsWellFormedUriString(variantDto.CustomUrl, UriKind.Absolute))
            {
                throw new ArgumentException("The provided Custom URL is not valid");
            }

            _mapper.Map(variantDto, existingVariant);

            await _unitOfWork.PromoteUrlvariants.UpdateAsync(existingVariant);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PromoteUrlvariantDto>(existingVariant);
        }

        public async Task DeletePromoteUrlvariantAsync(int id)
        {
            var variant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(id);
            if (variant == null)
            {
                throw new KeyNotFoundException($"PromoteUrlvariant with ID {id} not found");
            }

            await _unitOfWork.PromoteUrlvariants.DeleteAsync(variant);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ActivatePromoteUrlvariantAsync(int id)
        {
            var variant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(id);
            if (variant == null)
            {
                throw new KeyNotFoundException($"PromoteUrlvariant with ID {id} not found");
            }

            variant.IsActive = true;
            await _unitOfWork.PromoteUrlvariants.UpdateAsync(variant);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivatePromoteUrlvariantAsync(int id)
        {
            var variant = await _unitOfWork.PromoteUrlvariants.GetByIdAsync(id);
            if (variant == null)
            {
                throw new KeyNotFoundException($"PromoteUrlvariant with ID {id} not found");
            }

            variant.IsActive = false;
            await _unitOfWork.PromoteUrlvariants.UpdateAsync(variant);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PromoteUrlvariantExistsAsync(int id)
        {
            return await _unitOfWork.PromoteUrlvariants.ExistsAsync(p => p.VariantId == id);
        }

        public async Task<int> GetPromoteUrlvariantCountAsync()
        {
            return await _unitOfWork.PromoteUrlvariants.CountAsync(p => true);
        }

        public async Task<int> GetPromoteUrlvariantCountByPromoteAsync(int promoteId)
        {
            return await _unitOfWork.PromoteUrlvariants.CountAsync(p => p.PromoteId == promoteId);
        }

        public async Task<int> GetPromoteUrlvariantCountByTrafficSourceAsync(int trafficSourceId)
        {
            return await _unitOfWork.PromoteUrlvariants.CountAsync(p => p.TrafficSourceId == trafficSourceId);
        }

        public async Task<IEnumerable<PromoteUrlvariantDto>> GetActivePromoteUrlvariantsAsync()
        {
            var variants = await _unitOfWork.PromoteUrlvariants.GetAllAsync(
                p => p.IsActive == true,
                p => p.Promote,
                p => p.TrafficSource
            );
            return _mapper.Map<IEnumerable<PromoteUrlvariantDto>>(variants);
        }
    }
}
