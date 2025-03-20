using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Conversion
{
    public class ConversionUpdateDto
    {
        public int ConversionId { get; set; }
        public string? Status { get; set; }
        public bool? IsUnique { get; set; }
        public bool? IsSuspicious { get; set; }
        public bool? IsFraud { get; set; }
        public string? RejectionReason { get; set; }
    }
}
