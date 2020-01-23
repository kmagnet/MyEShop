using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using MyEShop.DataAccess.InMemory;

namespace MyEShop.WebUI.Controllers
{
    public class CategoryManagerController : Controller
    {
        IContainer<Category> context;

        public CategoryManagerController(IContainer<Category> categoryContext)
        {
            // Category Manager Controller constructor to initialize
            // concrete implementation of category IContainer 
            // then stores it into context variable 

            context = categoryContext;
        }

        // GET: CategoryManager
        public ActionResult Index()
        {
            List<Category> categories = context.Container().ToList(); // creates an instance of the collection of all categories
            return View(categories); // returns this collection of categories to view as a list
        }

        public ActionResult CreateCategory()
        { // This is for the page where user enters the details of the category
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        { // This is where the details of the category are posted into storage

            if (!ModelState.IsValid) // This ensures all the data entered into the model is correct 
            {                        // otherwise we need to return to the page and show validation
                                     // for the user
                return View(category);
            }
            else
            {
                context.Insert(category); // Insert the category in the category container
                context.Confirm(); // conrfirms the insertion of category into the memory

                return RedirectToAction("Index"); // Redirections user to the index page upon completion of this task
            }
        }

        public ActionResult EditCategory(string Id)
        { // This is the page where user selects which category is for revision

            Category category = context.Search(Id);

            if (category == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when category Id is not found
            }
            else
            {
                return View(category); // Return to view the category that we have found
            }
        }

        [HttpPost]
        public ActionResult EditCategory(Category category, string Id)
        { // This is where the actual revision occurs

            Category categoryToEdit = context.Search(Id);

            if (categoryToEdit == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when category Id is not found
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(category); // If there are validation errors on the edit request 
                                          // then return to view with the same category so that 
                                          // user can correct the revision errors
                }

                categoryToEdit.CategoryName = category.CategoryName; // Push the revisions into the category to be updated from the list
                
                context.Confirm(); // Push all the changes into the stored categories list in the cache

                return RedirectToAction("Index"); // Return to main page
            }
        }

        public ActionResult DeleteCategory(string Id) // This is where user selects which Category to delete
        {

            Category categoryToDelete = context.Search(Id);

            if (categoryToDelete == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when Category Id is not found
            }
            else
            {
                return View(categoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public ActionResult ConfirmDeleteCategory(string Id)
        {                                                   // Once user has selected the Category to delete
                                                            // then user needs to confirm the Category deletion
                                                            // and this method processes the confirmation of deletion

            Category categoryToDelete = context.Search(Id);

            if (categoryToDelete == null)
            {
                return HttpNotFound(); // Returns an HTTP Not Found error when Category Id is not found
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