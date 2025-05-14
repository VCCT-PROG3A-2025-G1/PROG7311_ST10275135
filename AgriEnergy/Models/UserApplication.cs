using Microsoft.AspNetCore.Identity;

namespace AgriEnergy.Models
{
    public class UserApplication : IdentityUser
    {
        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
