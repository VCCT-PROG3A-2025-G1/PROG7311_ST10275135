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

    // =======================
    // FARMER: Add Product
    // =======================
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
        var user = await _userManager.GetUserAsync(User);

        if (ModelState.IsValid && user != null)
        {
            product.FarmerId = user.Id;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("FarmerProductList");
        }

        return View(product);
    }

    // =======================
    // FARMER: View Own Products
    // =======================
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> FarmerProductList()
    {
        var user = await _userManager.GetUserAsync(User);
        var products = await _context.Products
            .Where(p => p.FarmerId == user.Id)
            .ToListAsync();

        return View(products);
    }

    // =======================
    // EMPLOYEE: Filter Products
    // =======================
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> EmployeeProductList(string category, DateTime? startDate, DateTime? endDate)
    {
        var products = _context.Products.Include(p => p.Farmer).AsQueryable();

        if (!string.IsNullOrEmpty(category))
            products = products.Where(p => p.Category == category);

        if (startDate.HasValue)
            products = products.Where(p => p.ProductionDate >= startDate);

        if (endDate.HasValue)
            products = products.Where(p => p.ProductionDate <= endDate);

        var result = await products.ToListAsync();
        return View(result);
    }
}
