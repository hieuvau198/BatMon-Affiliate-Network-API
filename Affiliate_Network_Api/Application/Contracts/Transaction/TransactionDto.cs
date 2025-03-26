using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Transaction
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string? Notes { get; set; }
        public string? TransactionRef { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
    }
}
