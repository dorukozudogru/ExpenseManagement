﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class ExpenseDocuments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Expenses))]
        public int ExpenseId { get; set; }

        [DisplayName("Fatura Görüntüsü")]
        public byte[] InvoiceImage { get; set; }

        [DisplayName("Fatura Görüntüsü Formatı")]
        public string InvoiceImageFormat { get; set; }
    }
}
