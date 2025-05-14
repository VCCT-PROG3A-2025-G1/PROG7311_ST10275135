using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AgriEnergy.Data;
using System.Diagnostics;
using AgriEnergy.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<UserApplication> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<UserApplication> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Farmer"))
            {
                return RedirectToAction("MyProducts", "Farmer");
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                return RedirectToAction("AddFarmer", "Employee");
            }
        }

        return View(); // for unauthenticated users
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
