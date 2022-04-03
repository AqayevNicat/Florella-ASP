using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class BasketController : Controller
    {
        private readonly FlorellaDbContext _context;
        public BasketController(FlorellaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            if (cookie != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }
            foreach (BasketVM item in basketVMs)
            {
                item.Id = _context.Products.FirstOrDefault(x => x.Id == item.Id).Id;
                item.Image = _context.Products.FirstOrDefault(x => x.Id == item.Id).Image;
                item.Price = _context.Products.FirstOrDefault(x => x.Id == item.Id).Price;
                item.Name = _context.Products.FirstOrDefault(x => x.Id == item.Id).Name;
                item.CategoryId = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == item.Id).Category.Id;
            }
            return View(basketVMs);
        }
        public IActionResult ChangeProductCount(int? id, int count)
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            if (cookie != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            }
            basketVMs.Find(x => x.Id == id).Count = count;
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketVMs));
            foreach (BasketVM item in basketVMs)
            {
                item.Id = _context.Products.FirstOrDefault(x => x.Id == item.Id).Id;
                item.Image = _context.Products.FirstOrDefault(x => x.Id == item.Id).Image;
                item.Price = _context.Products.FirstOrDefault(x => x.Id == item.Id).Price;
                item.Name = _context.Products.FirstOrDefault(x => x.Id == item.Id).Name;
                item.CategoryId = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == item.Id).Category.Id;
            }

            return PartialView("_BasketTablePartial", basketVMs);
        }
    }
}
