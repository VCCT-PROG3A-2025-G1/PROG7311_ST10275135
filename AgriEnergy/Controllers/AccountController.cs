using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergy.Models;
using AgriEnergy.ViewModels;
using AgriEnergy.Data;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<UserApplication> _userManager;
    private readonly SignInManager<UserApplication> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(
        UserManager<UserApplication> userManager,
        SignInManager<UserApplication> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new UserApplication
        {
            UserName = model.Email,
            Email = model.Email,
            Role = model.Role
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.Role);
            await _signInManager.SignInAsync(user, isPersistent: false);

            if (model.Role == "Farmer")
            {
                var farmer = new Farmer
                {
                    Name = model.Name ?? "Unnamed",
                    Location = model.Location ?? "Unknown",
                    UserId = user.Id
                };

                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                return RedirectToAction("FarmerProducts", "Farmer");
            }

            if (model.Role == "Employee")
            {
                return RedirectToAction("Dashboard", "Employee");
            }

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Farmer"))
                return RedirectToAction("Dashboard", "Farmer");

            if (roles.Contains("Employee"))
                return RedirectToAction("Dashboard", "Employee");

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}
