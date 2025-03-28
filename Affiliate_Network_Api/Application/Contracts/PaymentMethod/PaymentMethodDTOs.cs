﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.PaymentMethod
{
    public class PaymentMethodDto
    {
        public int MethodId { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? AddedDate { get; set; }
    }

    public class CreatePaymentMethodDto
    {
        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? AddedDate { get; set; }
    }

    public class UpdatePaymentMethodDto
    {
        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PaymentMethodFilterDto
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
