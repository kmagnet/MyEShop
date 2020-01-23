using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using MyEShop.Core.ViewModels;
using MyEShop.DataAccess.InMemory;

namespace MyEShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IContainer<Product> context;
        IContainer<Category> categories;

        public ProductManagerController(IContainer<Product> productContext, IContainer<Category> categoryContext)
        { 
            // Product Manager Controller constructor to initialize
            // concrete implementation of product container and category IContainers 
            // then stores them into their respective context variables

            context = productContext;
            categories = categoryContext;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Container().ToList(); // creates an instance of the collection of all products
            return View(products); // returns this collection of products to view as a list
        }

        public ActionResult CreateProduct()
        { 
            // This is for the page where user enters the details of the product

            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.Categories = categories.Container(); // Pulls the list of categories from database to appear on view as dropdown list
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase file)
        { 
            // This is where the details of the product are posted into storage

            if (!ModelState.IsValid) // This ensures all the data entered into the model is correct 
            {                        // otherwise we need to return to the page and show validation
                                     // for the user
                return View(product);
            }
            else
            {
                if (file != null) {
                    /// Validates if image file exists
                    /// and if so then saves the image using its
                    /// product ID because chances are user may upload 
                    /// images of the same filename so this ensures that 
                    /// we always name our file with a unique filename
                    
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image); // Saves the file in that folder context/productimages with the
                                                                                               // resulting filename string stored in product.Image
                }                

                context.Insert(product); // Insert the product in the product container
                context.Confirm(); // conrfirms the insertion of product into the memory

                return RedirectToAction("Index"); // Redirections user to the index page upon completion of this task
            }
        }

        public ActionResult EditProduct(string Id)
        { // This is the page where user selects which product is for revision

            Product product = context.Search(Id);

            if (product == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when product Id is not found
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.Categories = categories.Container();

                return View(viewModel); // Return to view the product that we have found
            }
        }

        [HttpPost]
        public ActionResult EditProduct(Product product, string Id, HttpPostedFileBase file)
        { // This is where the actual revision occurs

            Product productToEdit = context.Search(Id);

            if (productToEdit == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when product Id is not found
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product); // If there are validation errors on the edit request 
                                          // then return to view with the same product so that 
                                          // user can correct the revision errors
                }

                if (file != null)
                {
                    /// Validates if image file exists
                    /// and if so then saves the image using its
                    /// product ID because chances are user may upload 
                    /// images of the same filename so this ensures that 
                    /// we always name our file with a unique filename

                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image); // Saves the file in that folder context/productimages with the
                                                                                               // resulting filename string stored in product.Image
                }

                productToEdit.Category = product.Category; // Push the revisions into the product to be updated from the list
                productToEdit.Description = product.Description;
                productToEdit.Name = product.Name;                
                productToEdit.Price = product.Price;

                context.Confirm(); // Push all the changes into the stored products list in the cache

                return RedirectToAction("Index"); // Return to main page
            }
        }

        public ActionResult DeleteProduct(string Id) // This is where user selects which product to delete
        {

            Product productToDelete = context.Search(Id);

            if (productToDelete == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when product Id is not found
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public ActionResult ConfirmDeleteProduct(string Id)
        {                                                   // Once user has selected the product to delete
                                                            // then user needs to confirm the product deletion
                                                            // and this method processes the confirmation of deletion

            Product productToDelete = context.Search(Id);

            if (productToDelete == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when product Id is not found
            }
            else
            {
                context.Delete(Id);
                context.Confirm();
                return RedirectToAction("Index");
            }
        }
    }
}