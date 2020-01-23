using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IContainer<Product> context;
        IContainer<Category> categories;

        public HomeController(IContainer<Product> productContext, IContainer<Category> categoryContext)
        {
            // Product Manager Controller constructor to initialize
            // concrete implementation of product container and category IContainers 
            // then stores them into their respective context variables

            context = productContext;
            categories = categoryContext;
        }

        public ActionResult Index()
        {
            List<Product> products = context.Container().ToList();
            return View(products);
        }

        public ActionResult ViewProductDetails(string Id) {
            
            Product product = context.Search(Id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}