using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ExpenseManagement.Models
{
    public class CarModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Model Adı")]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(CarBrands))]
        public int CarBrandId { get; set; }
        [NotMapped]
        [DisplayName("Marka Adı")]
        public string CarBrandName { get; set; }
        public virtual CarBrands CarBrand { get; set; }

        [JsonIgnore]
        public virtual List<VehiclePurchases> VehiclePurchases { get; set; }
    }
}
