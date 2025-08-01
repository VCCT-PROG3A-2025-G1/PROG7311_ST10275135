using AgriEnergy.Data;
using AgriEnergy.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergy.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var posts = _context.ForumModels
                                .OrderByDescending(p => p.CreatedAt)
                                .ToList();
            return View(posts);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ForumModels model)
        {
            if (ModelState.IsValid)
            {
                model.Author = User.Identity?.Name ?? "Anonymous";
                model.CreatedAt = DateTime.Now;

                _context.ForumModels.Add(model);
                _context.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}