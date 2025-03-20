using Application.Contracts.FraudType;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FraudTypeService : IFraudTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FraudTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<FraudTypeDto>> GetAllFraudTypesAsync()
        {
            var fraudTypes = await _unitOfWork.FraudTypes.GetAllAsync(
                includes: new System.Linq.Expressions.Expression<Func<FraudType, object>>[]
                {
                    ft => ft.FraudCases
                });

            return _mapper.Map<IEnumerable<FraudTypeDto>>(fraudTypes);
        }

        public async Task<FraudTypeDto> GetFraudTypeByIdAsync(int id)
        {
            var fraudType = await _unitOfWork.FraudTypes.GetByIdAsync(id,
                ft => ft.FraudCases);

            if (fraudType == null)
            {
                throw new KeyNotFoundException($"FraudType with ID {id} not found");
            }

            return _mapper.Map<FraudTypeDto>(fraudType);
        }

        public async Task<FraudTypeDto> CreateFraudTypeAsync(FraudTypeCreateDto fraudTypeDto)
        {
            // Validation checks
            if (string.IsNullOrWhiteSpace(fraudTypeDto.Name))
            {
                throw new ArgumentException("Name is required");
            }

            var existingFraudType = await _unitOfWork.FraudTypes.FindAsync(ft => ft.Name == fraudTypeDto.Name);
            if (existingFraudType != null)
            {
                throw new ArgumentException($"FraudType with name '{fraudTypeDto.Name}' already exists");
            }

            if (fraudTypeDto.SeverityLevel.HasValue)
            {
                if (fraudTypeDto.SeverityLevel < 1 || fraudTypeDto.SeverityLevel > 10)
                {
                    throw new ArgumentException("SeverityLevel must be between 1 and 10");
                }
            }

            // Map DTO to entity
            var fraudType = _mapper.Map<FraudType>(fraudTypeDto);

            // Create the fraud type
            var createdFraudType = await _unitOfWork.FraudTypes.CreateAsync(fraudType);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the created fraud type with related data for response
            var result = await _unitOfWork.FraudTypes.GetByIdAsync(createdFraudType.FraudTypeId,
                ft => ft.FraudCases);

            return _mapper.Map<FraudTypeDto>(result);
        }

        public async Task<FraudTypeDto> UpdateFraudTypeAsync(FraudTypeUpdateDto fraudTypeDto)
        {
            var existingFraudType = await _unitOfWork.FraudTypes.GetByIdAsync(fraudTypeDto.FraudTypeId,
                ft => ft.FraudCases);

            if (existingFraudType == null)
            {
                throw new KeyNotFoundException($"FraudType with ID {fraudTypeDto.FraudTypeId} not found");
            }

            // Validation checks
            if (!string.IsNullOrWhiteSpace(fraudTypeDto.Name))
            {
                var duplicateFraudType = await _unitOfWork.FraudTypes.FindAsync(ft => ft.Name == fraudTypeDto.Name && ft.FraudTypeId != fraudTypeDto.FraudTypeId);
                if (duplicateFraudType != null)
                {
                    throw new ArgumentException($"FraudType with name '{fraudTypeDto.Name}' already exists");
                }
            }

            if (fraudTypeDto.SeverityLevel.HasValue)
            {
                if (fraudTypeDto.SeverityLevel < 1 || fraudTypeDto.SeverityLevel > 10)
                {
                    throw new ArgumentException("SeverityLevel must be between 1 and 10");
                }
            }

            // Map updated properties
            _mapper.Map(fraudTypeDto, existingFraudType);

            await _unitOfWork.FraudTypes.UpdateAsync(existingFraudType);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FraudTypeDto>(existingFraudType);
        }

        public async Task DeleteFraudTypeAsync(int id)
        {
            var fraudType = await _unitOfWork.FraudTypes.GetByIdAsync(id);
            if (fraudType == null)
            {
                throw new KeyNotFoundException($"FraudType with ID {id} not found");
            }

            // Check if there are related FraudCases
            var fraudCaseCount = await _unitOfWork.FraudCases.CountAsync(fc => fc.FraudTypeId == id);
            if (fraudCaseCount > 0)
            {
                throw new InvalidOperationException($"Cannot delete FraudType with ID {id} because it is associated with {fraudCaseCount} FraudCase(s)");
            }

            await _unitOfWork.FraudTypes.DeleteAsync(fraudType);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}