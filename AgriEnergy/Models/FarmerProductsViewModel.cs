using System.Collections.Generic;

namespace AgriEnergy.Models
{
    public class FarmerProductsViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public string? FarmerName { get; set; }
    }
}


