using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.ConversionType
{
    public class ConversionTypeCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? TrackingMethod { get; set; }

        public bool RequiresApproval { get; set; }

        [StringLength(50)]
        public string? ActionType { get; set; }
    }
}
