using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlorellaDbContext _context;
        public HomeController(FlorellaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products =  await _context.Products.Include(p=>p.Category).ToListAsync();
            List<Category> categories = await _context.Categories.ToListAsync();

            HomeVM homeVM = new HomeVM
            {
                Product = products,
                Category = categories
            };
            return View(homeVM);
        }
        public async Task<IActionResult> AddToBasket(int? id)
        {
            // Check Id
            if (id == null) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();


            BasketVM basketVM = new BasketVM
            {
                Id = _context.Products.FirstOrDefault(x => x.Id == id).Id,
                Count = 1
            };


            // Append to cookie
            List<BasketVM> basketVMs = null;
            string cookie = HttpContext.Request.Cookies["basket"];
            if (cookie != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                if (basketVMs.Any(x=>x.Id==id))
                {
                    basketVMs.FirstOrDefault(x => x.Id == id).Count += 1;
                }
                else
                {
                    basketVMs.Add(basketVM);
                }
            }
            else
            {
                basketVMs = new List<BasketVM>();
                basketVMs.Add(basketVM);
            }

            string prod = JsonConvert.SerializeObject(basketVMs);
            //HttpContext.Session.SetString("salam", prod);
            HttpContext.Response.Cookies.Append("basket", prod);
            foreach (BasketVM item in basketVMs)
            {
                item.Id = _context.Products.FirstOrDefault(x => x.Id == item.Id).Id;
                item.Image = _context.Products.FirstOrDefault(x => x.Id == item.Id).Image;
                item.Name = _context.Products.FirstOrDefault(x => x.Id == item.Id).Name;
                item.CategoryId = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == item.Id).Category.Id;
            }
            return PartialView("_CartIndexPartial", basketVMs);
        }
        //public IActionResult GetSession()
        //{
        //    return Json(JsonConvert.DeserializeObject( HttpContext.Session.GetString("salam")));
        //}
    }
}
