using Application.Contracts.Transaction;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<TransactionCreateDto, Transaction>()
                .ForMember(dest => dest.PaymentDate,
                    opt => opt.MapFrom(src => src.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow)));
        }
    }
}
