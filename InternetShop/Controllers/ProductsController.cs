using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShop.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace InternetShop.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        ProductsContext db;
        public ProductsController(ProductsContext context)
        {
            this.db = context;

            if (!db.Products.Any())
            {
                db.Products.Add(new Product { Name = "Galaxy S10 Plus", Categories = "smartphone", Price = 25999, Producer = "Samsung" });
                db.Products.Add(new Product { Name = "iPhone 11", Categories = "smartphone", Price = 28499, Producer = "Apple" });
                db.Products.Add(new Product { Name = "16", Categories = "smartphone", Price = 10000, Producer = "Meizu" });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return db.Products.ToList() ;
        }


        [HttpGet("{json}")]
        public IEnumerable<Product> Get(string json)
        {
            var products = db.Products.ToList();

            JObject filter = JObject.Parse(json);
            
            if (filter["categories"] != null)
            {
                List<string> categories = filter["categories"].Select(f => (string)f).ToList();

                List<Product> prod2 = new List<Product>();
                foreach (var cat in categories)
                {
                    prod2.Union(products.Where(p => p.Categories.Contains(cat)).ToList());
                }

                products = prod2;
            }

            if (filter["producer"] != null)
            {
                List<string> producer = filter["producer"].Select(f => (string)f).ToList();
                products = products.Where(p => producer.Contains(p.Producer)).ToList();
            }

            if (filter["price"] != null)
            {
                int priceFrom = filter["price"].Select(p => (int)p).ToList()[0];
                int priceTo = filter["price"].Select(p => (int)p).ToList()[1];
                products = products.Where(p => p.Price >= priceFrom && p.Price <= priceTo).ToList();
            }


            if (filter["name"] != null)
            {
                string name = filter["name"].ToString();
                products = products.Where(p => p.Name.Contains(name)).ToList();
            }

            return products;
        }


        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
                return BadRequest();

            db.Products.Add(product);
            db.SaveChanges();
            return Ok(product);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (!db.Products.Any(x => x.Id == product.Id))
            {
                return NotFound();
            }

            db.Update(product);
            db.SaveChanges();
            return Ok(product);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();
            return Ok(product);
        }
    }
}
