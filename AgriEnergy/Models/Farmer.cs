using System.Collections.Generic;
using AgriEnergy.Models;

namespace AgriEnergy.Models
{
    public class Farmer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

       public string UserId { get; set; } = string.Empty;
       public UserApplication? User { get; set; }

       public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
