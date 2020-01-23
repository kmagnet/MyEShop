using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using MyEShop.Core.ViewModels;
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

        public ActionResult Index(string Category=null)
        {   
            /// This index method accepts an optional parameter
            /// where you can have a null item parameter passed or
            /// if you don't pass any parameters then it is assumed null
            
            List<Product> products = context.Container().ToList();
            List<Category> categories = this.categories.Container().ToList();

            /// if Category is found to be null then we just return the product list
            /// as it is otherwise we will return a filtered product list
            if (Category == null)
            {
                products = context.Container().ToList();
            }
            else {
                products = context.Container().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.Categories = categories;
            
            return View(model);
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