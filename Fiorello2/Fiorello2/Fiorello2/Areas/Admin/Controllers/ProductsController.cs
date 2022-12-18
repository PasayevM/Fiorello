using Fiorello2.DAL;
using Fiorello2.Helpers;
using Fiorello2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products.Include(x=>x.Category).ToListAsync(); 
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
             return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, int catID)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            #region Save Photo

            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "Bu hisse dolu olmalidir.");
                return View();
            }

            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Yalniz wekil formati.");
                return View();
            }

            if (product.Photo.IsOlder2Mb())
            {
                ModelState.AddModelError("Photo", "Max 2MB.");
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "img");
            product.Image = await product.Photo.SaveImageAsync(folder);

            #endregion
            product.CategoryId = catID;
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

         public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Categories = await _db.Categories.ToListAsync();

            return View(dbProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, int catID)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }

            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(dbProduct);
            }
            #region Save Photo Update

            if (product.Photo != null)
            {
                if (!product.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Yalniz wekil formati.");
                    return View(dbProduct);
                }

                if (product.Photo.IsOlder2Mb())
                {
                    ModelState.AddModelError("Photo", "Max 2MB.");
                    return View(dbProduct);
                }

                string folder = Path.Combine(_env.WebRootPath, "img");
                string path = Path.Combine(folder, dbProduct.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                dbProduct.Image = await product.Photo.SaveImageAsync(folder);
            }


            #endregion
            dbProduct.CategoryId = catID;
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            if (dbProduct.IsDeactive == true)
            {
                dbProduct.IsDeactive = false;

            }
            else
            {
                dbProduct.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
