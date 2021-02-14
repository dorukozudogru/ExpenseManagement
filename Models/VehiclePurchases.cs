﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class VehiclePurchases
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(CarModel))]
        public int CarModelId { get; set; }
        public virtual CarModels CarModel { get; set; }
        [NotMapped]
        [DisplayName("Marka")]
        public string CarBrandName { get; set; }

        [DisplayName("Araç Sıfır Mı?")]
        public bool IsNew { get; set; }

        [DisplayName("Araç Satıldı Mı?")]
        public bool IsSold { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Satış Tarihi")]
        public DateTime? SaleDate { get; set; }

        [Required]
        [DisplayName("Şase No")]
        public string Chassis { get; set; }

        [NotMapped]
        private string _fullInfo { get; set; }
        [NotMapped]
        [DisplayName("Araç Bilgileri")]
        public string FullInfo
        {
            get
            {
                if (string.IsNullOrEmpty(_fullInfo))
                {
                    return string.Format($"{this.Chassis}");
                }
                return _fullInfo;
            }
            set
            {
                _fullInfo = value;
            }
        }

        [Required]
        [DisplayName("Alış Fiyatı")]
        public double PurchaseAmount { get; set; }

        [Required]
        [DisplayName("Alış Fiyatı Dövizi")]
        public byte PurchaseAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Alış Fiyatı Dövizi")]
        public string PurchaseAmountCurrencyName { get; set; }

        [DisplayName("Satış Fiyatı")]
        public double? SaleAmount { get; set; }

        [Required]
        [DisplayName("Satış Fiyatı Dövizi")]
        public byte? SaleAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Satış Fiyatı Dövizi")]
        public string SaleAmountCurrencyName { get; set; }
    }
}