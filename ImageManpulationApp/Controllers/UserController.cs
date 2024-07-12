using ImageManpulationApp.Data;
using ImageManpulationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageManpulationApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public UserController(AppDbContext con, IWebHostEnvironment env)
        {
            _context = con;
            _env = env;
        }

        public IActionResult Index()
        {
            var data = _context.Users.ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null && file != null && file.Length > 0)
            {
                var oldFilePath = Path.Combine(_env.WebRootPath, "uploads", user.ImageName);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                var newFilePath = Path.Combine(_env.WebRootPath, "uploads", file.FileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.ImageName = file.FileName;
                user.ImagePath = newFilePath;
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Create(User user, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_env.WebRootPath, "uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                user.ImageName = file.FileName;
                user.ImagePath = filePath;
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                var filePath = Path.Combine(_env.WebRootPath, "uploads", user.ImageName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
