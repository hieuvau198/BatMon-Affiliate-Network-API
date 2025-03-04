using System;
using System.Collections.Generic;

namespace Application.Contracts.Publisher
{
    public class PublisherDto
    {
        public int PublisherId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TaxId { get; set; }
        public DateOnly? RegistrationDate { get; set; }
        public bool? IsActive { get; set; }
        public string? ReferredByCode { get; set; }
        public string? ReferralCode { get; set; }
        public int Role { get; set; }

        // Additional properties to display without loading entire related objects
        public int TotalTrafficSources { get; set; }
        public int TotalCampaigns { get; set; }
    }
}