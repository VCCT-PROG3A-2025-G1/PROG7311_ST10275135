using AgriEnergy.Data;
using AgriEnergy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserApplication> _userManager;

    public ProductController(ApplicationDbContext context, UserManager<UserApplication> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "Farmer")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Farmer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Product product)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

        if (farmer == null)
        {
            // Redirect to error page if the farmer is not found
            return RedirectToAction("Error", "Home");
        }

        product.FarmerId = farmer.Id;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("FarmerProductList", "Farmer");
    }

    [HttpGet]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> EmployeeProductList(string category, DateTime? startDate, DateTime? endDate)
    {
        var products = _context.Products
            .Include(p => p.Farmer)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
            products = products.Where(p => p.Category == category);

        if (startDate.HasValue)
            products = products.Where(p => p.ProductionDate >= startDate.Value);

        if (endDate.HasValue)
            products = products.Where(p => p.ProductionDate <= endDate.Value);

        var result = await products.ToListAsync();
        return View(result);
    }
}
