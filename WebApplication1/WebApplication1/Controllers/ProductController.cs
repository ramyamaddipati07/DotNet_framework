using System.Collections.Generic;
using System.Web.Mvc;
using YourProjectName.Models; // Potential issue: unused import if Product isn't used correctly

namespace YourProjectName.Controllers
{
    public class ProductsController : Controller
    {
        // Issue: Public static mutable list can cause threading issues
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Pen", Price = 10 },
            new Product { Id = 2, Name = "Notebook", Price = 25 }
        };

        // GET: /Products
        public ActionResult Index()
        {
            return View(products); // OK
        }

        // GET: /Products/Details/{id}
        public ActionResult Details(int id)
        {
            // Issue: No null check for products list (edge case)
            var product = products.Find(p => p.Id == id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // GET: /Products/Create
        public ActionResult Create()
        {
            return View(); // No issues
        }

        // POST: /Products/Create
        [HttpPost]
        // Issue: Missing [ValidateAntiForgeryToken] (security vulnerability)
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Issue: Naive ID assignment can break if items are deleted
                product.Id = products.Count + 1;
                products.Add(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: /Products/Edit/{id}
        public ActionResult Edit(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Products/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product updatedProduct)
        {
            var product = products.Find(p => p.Id == updatedProduct.Id);
            if (product == null)
                return HttpNotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            return RedirectToAction("Index");
        }

        // GET: /Products/Delete/{id}
        public ActionResult Delete(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: /Products/Delete/{id}
        [HttpPost, ActionName("Delete")]
        // Issue: Missing [ValidateAntiForgeryToken] again
        public ActionResult DeleteConfirmed(int id)
        {
            var product
