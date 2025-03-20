using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.FraudReport
{
    public class FraudReportReadDto
    {
        public int ReportId { get; set; }
        public bool IsRead { get; set; }
    }
}
