using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Currency
{
    public class CurrencyDto
    {
        public string CurrencyCode { get; set; } = null!;
        public string? CurrencyName { get; set; }
        public decimal? ExchangeRate { get; set; }
    }

    public class CreateCurrencyDto
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CurrencyCode { get; set; } = null!;

        [StringLength(50)]
        public string? CurrencyName { get; set; }

        [Range(0.0001, 1000)]
        public decimal? ExchangeRate { get; set; }
    }

    public class UpdateCurrencyDto
    {
        [StringLength(50)]
        public string? CurrencyName { get; set; }

        [Range(0.0001, 1000)]
        public decimal? ExchangeRate { get; set; }
    }
}
