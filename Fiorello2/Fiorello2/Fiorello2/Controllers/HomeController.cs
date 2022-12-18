using Fiorello2.DAL;
using Fiorello2.Models;
using Fiorello2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Products = await _db.Products.Where(x => x.IsDeactive == false).ToListAsync(),
                Categories = await _db.Categories.Where(x => x.IsDeactive == false).ToListAsync(),
               
            };
            return View(homeVM);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
