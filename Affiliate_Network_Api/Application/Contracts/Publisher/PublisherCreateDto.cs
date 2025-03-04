using System;

namespace Application.Contracts.Publisher
{
    public class PublisherCreateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TaxId { get; set; }
        public string? ReferredByCode { get; set; }
        public int Role { get; set; } = 1; // Default role
    }
}