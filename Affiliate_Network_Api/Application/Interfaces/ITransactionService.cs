using Application.Contracts.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionDto>> GetTransactionsBySenderIdAsync(int senderId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByReceiverIdAsync(int receiverId);
        Task<TransactionDto> CreateTransactionAsync(TransactionCreateDto transactionDto);
        Task<bool> TransactionExistsAsync(int id);
        Task<int> GetTransactionCountAsync();
        Task<decimal> GetTotalTransactionAmountAsync();
        Task<decimal> GetTotalTransactionAmountBySenderAsync(int senderId);
        Task<decimal> GetTotalTransactionAmountByReceiverAsync(int receiverId);
    }
}
