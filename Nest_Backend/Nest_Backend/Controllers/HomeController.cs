using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = await _context.Sliders.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Recently = await _context.Products.OrderByDescending(p => p.Id).Take(3).Include(p => p.ProductImgs).Include(p => p.Categories).ToListAsync(),
                Products = await _context.Products.OrderByDescending(p => p.StockCount).Where(p=>p.StockCount>0).Take(10).Include(p => p.ProductImgs).Include(p => p.Categories).ToListAsync(),
                TopRated = await _context.Products.OrderByDescending(p => p.Raiting).Take(3).Include(p => p.ProductImgs).Include(p => p.Categories).ToListAsync()

            };
            return View(homeVM);
        }
    }
}
