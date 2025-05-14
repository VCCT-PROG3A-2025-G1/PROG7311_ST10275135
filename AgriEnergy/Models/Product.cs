using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergy.Models
{
    public class Product
    {
        public int ProductId { get; set; }

       
        public string Name { get; set; } = string.Empty;


        public string Category { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public string FarmerId { get; set; }
        public Farmer Farmer { get; set; } = null!;
    }
}
