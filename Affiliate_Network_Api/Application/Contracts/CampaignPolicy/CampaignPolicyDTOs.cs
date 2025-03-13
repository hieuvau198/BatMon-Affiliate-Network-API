using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.CampaignPolicy
{
    public class CampaignPolicyDto
    {
        public int PolicyId { get; set; }
        public string? PolicyName { get; set; }
        public string? Description { get; set; }
        public string? PenaltyInfo { get; set; }
        public string? AppliedTo { get; set; }
    }

    public class CampaignPolicyCreateDto
    {
        [MaxLength(255)]
        public string? PolicyName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? PenaltyInfo { get; set; }

        [MaxLength(255)]
        public string? AppliedTo { get; set; }
    }

    public class CampaignPolicyUpdateDto
    {
        public int PolicyId { get; set; }

        [MaxLength(255)]
        public string? PolicyName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? PenaltyInfo { get; set; }

        [MaxLength(255)]
        public string? AppliedTo { get; set; }
    }
}
