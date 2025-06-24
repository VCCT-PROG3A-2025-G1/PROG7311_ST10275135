using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgriEnergy.Models;
using AgriEnergy.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context) => _context = context;

    public IActionResult FarmerList()
    {
        var farmers = _context.Farmers.ToList();
        return View(farmers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFarmer(Farmer farmer)
    {
        if (!ModelState.IsValid)
        {
            return View(farmer);
        }

        _context.Farmers.Add(farmer);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Failed to add farmer. Ensure the User ID exists.");
            return View(farmer);
        }

        return RedirectToAction("Dashboard", "Employee");
    }

    [HttpGet]
    public IActionResult AddFarmer()
    {
        var users = _context.Users
            .Where(u => u.Role == "Farmer")
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.Email
            })
            .ToList();

        ViewBag.UserList = users;
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult ViewFarmers()
    {
        var farmers = _context.Farmers.ToList();
        return View(farmers);
    }

    public IActionResult FilterProducts(string category, DateTime? startDate, DateTime? endDate)
    {
        var products = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            products = products.Where(p => p.Category == category);

        if (startDate.HasValue)
            products = products.Where(p => p.ProductionDate >= startDate.Value);

        if (endDate.HasValue)
            products = products.Where(p => p.ProductionDate <= endDate.Value);

        return View(products.ToList());
    }
}
