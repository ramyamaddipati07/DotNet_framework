using System.Collections.Generic;
using System.Web.Mvc;
using YourProjectName.Models; // If you have a Product model

namespace YourProjectName.Controllers
{
	public class ProductsController : Controller
	{
		// Dummy product list (simulate database)
  //checkt the comments
		private static List<Product> products = new List<Product>
		{
			new Product { Id = 1, Name = "Pen", Price = 10 },
			new Product { Id = 2, Name = "Notebook", Price = 25 }
		};

		// GET: Products
		public ActionResult Index()
		{
			return View(products);
		}

		// GET: Products/Details/1
		public ActionResult Details(int id)
		{
			var product = products.Find(p => p.Id == id);
			if (product == null)
				return HttpNotFound();

			return View(product);
		}

		// GET: Products/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		[HttpPost]
		public ActionResult Create(Product product)
		{
			if (ModelState.IsValid)
			{
				product.Id = products.Count + 1;
				products.Add(product);
				return RedirectToAction("Index");
			}
			return View(product);
		}

		// GET: Products/Edit/1
		public ActionResult Edit(int id)
		{
			var product = products.Find(p => p.Id == id);
			if (product == null)
				return HttpNotFound();

			return View(product);
		}

		// POST: Products/Edit/1
		[HttpPost]
		public ActionResult Edit(Product updatedProduct)
		{
			var product = products.Find(p => p.Id == updatedProduct.Id);
			if (product == null)
				return HttpNotFound();

			product.Name = updatedProduct.Name;
			product.Price = updatedProduct.Price;

			return RedirectToAction("Index");
		}
	}
}
