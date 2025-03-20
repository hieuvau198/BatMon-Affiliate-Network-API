using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.ConversionType
{
    public class ConversionTypeDto
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TrackingMethod { get; set; }
        public bool RequiresApproval { get; set; }
        public string? ActionType { get; set; }
        public int CampaignCount { get; set; } // Extra field for associated campaigns count
    }




}
