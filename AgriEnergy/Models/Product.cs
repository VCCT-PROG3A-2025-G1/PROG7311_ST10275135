using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public Farmer Farmer { get; set; } = null!;
    }
}
