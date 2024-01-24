using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFirstWebAppi.Models;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace MyFirstWebAppi.Controllers
{
    public class ProductController : Controller
    {

        private IEnumerable<ProductViewModel> products = new List<ProductViewModel>()
        {
            new ProductViewModel()
            {
                Id = 1,
                Name = "Cheese",
                Price = 7.00
            },
            new ProductViewModel()
            {
                Id = 2,
                Name = "Ham",
                Price = 5.00
            },
            new ProductViewModel()
            {
                Id = 3,
                Name = "Bread",
                Price = 1.50
            }
        };

        public IActionResult Index()
        {
            return View(products);
        }

        [ActionName("My-Products")]

        public IActionResult Filtered(string? keyword = null)
        {
            if (keyword == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = products
                .Where(p => p.Name.ToLower().Contains(keyword.ToLower()));

            return View(nameof(Index), model);
        }

        public IActionResult ById(int id)
        {
            var model = products.FirstOrDefault(p => p.Id == id);

            if (model == null)
            {
                TempData["Error"] = "No such product";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult AllAsJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            return Json(products, options);
        }

        public IActionResult AllAsText()
        {
            var text = string.Empty;
            foreach (var product in products)
            {
                text += $"Product {product.Id}: {product.Name} - {product.Price} lv";
                text += "\r\n";

            }
            return Content(text);
        }

        public IActionResult AllAsTextFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine($"Product: {product.Id}: {product.Name} - {product.Price:f2} lv.");
            }

            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(sb.ToString().TrimEnd()), "text/plain");
        }

        public IActionResult DownloadAll()
        {
            string content = GetAllProductsAsString();
            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(content), "text/plain");
        }

        public IActionResult All(string? keyword = null)
        {
            if (keyword == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = products
                .Where(p => p.Name.ToLower().Contains(keyword.ToLower()));

            return View(nameof(Index), model);
        }

        private string GetAllProductsAsString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in products)
            {
                stringBuilder.AppendLine($"Product {item.Id}: {item.Name} - {item.Price} lv.");
            }

            return stringBuilder.ToString();
        }

    }
}
