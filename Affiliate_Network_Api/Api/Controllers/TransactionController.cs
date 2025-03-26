using Application.Contracts.Transaction;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            return Ok(transaction);
        }

        [HttpGet("sender/{senderId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsBySenderId(int senderId)
        {
            var transactions = await _transactionService.GetTransactionsBySenderIdAsync(senderId);
            return Ok(transactions);
        }

        [HttpGet("receiver/{receiverId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByReceiverId(int receiverId)
        {
            var transactions = await _transactionService.GetTransactionsByReceiverIdAsync(receiverId);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransactionById), new { id = createdTransaction.TransactionId }, createdTransaction);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTransactionCount()
        {
            var count = await _transactionService.GetTransactionCountAsync();
            return Ok(count);
        }

        [HttpGet("total-amount")]
        public async Task<ActionResult<decimal>> GetTotalTransactionAmount()
        {
            var totalAmount = await _transactionService.GetTotalTransactionAmountAsync();
            return Ok(totalAmount);
        }

        [HttpGet("total-amount/sender/{senderId}")]
        public async Task<ActionResult<decimal>> GetTotalTransactionAmountBySender(int senderId)
        {
            var totalAmount = await _transactionService.GetTotalTransactionAmountBySenderAsync(senderId);
            return Ok(totalAmount);
        }

        [HttpGet("total-amount/receiver/{receiverId}")]
        public async Task<ActionResult<decimal>> GetTotalTransactionAmountByReceiver(int receiverId)
        {
            var totalAmount = await _transactionService.GetTotalTransactionAmountByReceiverAsync(receiverId);
            return Ok(totalAmount);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> TransactionExists(int id)
        {
            var exists = await _transactionService.TransactionExistsAsync(id);
            return Ok(exists);
        }
    }
}
