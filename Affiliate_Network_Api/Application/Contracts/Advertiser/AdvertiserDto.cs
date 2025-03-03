using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Advertiser
{
    public class AdvertiserDto
    {
        public int AdvertiserId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string TaxId { get; set; }
        public DateOnly? RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public int Role { get; set; }
    }
}
