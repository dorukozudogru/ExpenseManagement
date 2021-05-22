﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class Salesmans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Danışman Adı-Soyadı")]
        public string Name { get; set; }
    }
}
