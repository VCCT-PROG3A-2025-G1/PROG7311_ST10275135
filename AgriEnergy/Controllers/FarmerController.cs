using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergy.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergy.Models; 


[Authorize(Roles = "Farmer")]
public class FarmerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserApplication> _userManager;

    public FarmerController(ApplicationDbContext context, UserManager<UserApplication> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize(Roles = "Farmer")]
    public IActionResult AddProduct(Product product)
    {
        var userId = _userManager.GetUserId(User);
        var farmer = _context.Farmers.FirstOrDefault(f => f.UserId == userId);

        if (farmer == null)
        {
            // Redirect to error or force profile completion
            return RedirectToAction("Error");
        }

        product.FarmerId = farmer.Id.ToString();
        _context.Products.Add(product);
        _context.SaveChanges();

        return RedirectToAction("FarmerProducts");
    }


    [Authorize(Roles = "Farmer")]
   public IActionResult FarmerProducts()
{
    var currentUserId = _userManager.GetUserId(User);

    var products = _context.Products
        .Where(p => p.FarmerId == currentUserId)
        .ToList();

    var farmerName = _context.Users
        .Where(u => u.Id == currentUserId)
        .Select(u => u.UserName)
        .FirstOrDefault();

    var viewModel = new FarmerProductsViewModel
    {
        Products = products,
        FarmerName = farmerName
    };

    return View(viewModel);
}





// GET: Farmer/EditProduct/5
public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Farmer/EditProduct/5
    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("FarmerProducts");
        }
        return View(product);
    }

    // GET: Farmer/DeleteProduct/5
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("FarmerProducts");
    }
}
