using Application.Contracts.Transaction;
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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {id} not found");
            }

            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsBySenderIdAsync(int senderId)
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync(t => t.SenderId == senderId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByReceiverIdAsync(int receiverId)
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync(t => t.ReceiverId == receiverId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> CreateTransactionAsync(TransactionCreateDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);

            // Set default values
            transaction.PaymentDate = transactionDto.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var createdTransaction = await _unitOfWork.Transactions.CreateAsync(transaction);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved

            return _mapper.Map<TransactionDto>(createdTransaction);
        }

        public async Task<bool> TransactionExistsAsync(int id)
        {
            return await _unitOfWork.Transactions.ExistsAsync(t => t.TransactionId == id);
        }

        public async Task<int> GetTransactionCountAsync()
        {
            return await _unitOfWork.Transactions.CountAsync(t => true);
        }

        public async Task<decimal> GetTotalTransactionAmountAsync()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            return transactions.Sum(t => t.Amount);
        }

        public async Task<decimal> GetTotalTransactionAmountBySenderAsync(int senderId)
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync(t => t.SenderId == senderId);
            return transactions.Sum(t => t.Amount);
        }

        public async Task<decimal> GetTotalTransactionAmountByReceiverAsync(int receiverId)
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync(t => t.ReceiverId == receiverId);
            return transactions.Sum(t => t.Amount);
        }
    }
}
