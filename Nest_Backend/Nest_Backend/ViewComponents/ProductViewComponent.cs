using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_Backend.DAL;
using Nest_Backend.Models;
using Nest_Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_Backend.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private AppDbContext _context { get; }
        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync( int page = 1)
        {
            List<Product> products = await _context.Products.Where(p => p.IsDeleted == false)

                                    .OrderByDescending(p => p.Id)
                                    .Skip((page-1)*10)
                                    .Take(10)
                                    .Include(p => p.ProductImgs)
                                    .Include(p => p.Categories).ToListAsync();
            PaginateVM<Product> pagination = new PaginateVM<Product>
            {
                Items = products,
                ActivePage = page,
                PageCount = GetPageCount(products.Count)
            };

            return View(await Task.FromResult(pagination));
        }
        private int GetPageCount(int count)
        {
            return (int)Math.Ceiling((double)count/10);
        }
    }
}
