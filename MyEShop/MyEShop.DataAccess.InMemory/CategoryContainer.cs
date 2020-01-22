using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyEShop.Core.Models;

namespace MyEShop.DataAccess.InMemory
{
    public class CategoryContainer
    {
        ObjectCache cache = MemoryCache.Default; // Creates an ObjectCache containing the reference for the default MemoryCache instance which 
                                                 // serves as flexible temporary storage for data that my application can process
        List<Category> categories;

        public CategoryContainer()
        {
            categories = cache["categories"] as List<Category>; // When the constructor is launched, it will look into the cache to see if there
                                                           // is a cache called categories which is a list of categories but if that returns null
                                                           // then it creates a new list of categories
            if (categories == null)
            {
                categories = new List<Category>();
            }
        }

        public void Confirm()
        {
            cache["categories"] = categories; // When people add categories, we don't want them to save it straight away
                                          // so they will get stored first as a list before we are finally ready to move 
                                          // them into the cache then into the SQL database.
        }

        public void Insert(Category category)
        { // Simply adds a category into the list categories
            categories.Add(category);
        }

        public void Update(Category category)
        { // Finds the category to be updated via its Id
          // then stores its reference then once category is found
          // it then pushes the updates into the original categgory instance

            Category categoryToUpdate = categories.Find(c => c.Id == category.Id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate = category;
            }
            else
            {
                throw new Exception("Category does not exist!");
            }
        }

        public Category Search(string Id)
        { // Searches for a category by Id then returns the
          // object reference for the category requested

            Category category = categories.Find(c => c.Id == Id);

            if (category != null)
            {
                return category;
            }
            else
            {
                throw new Exception("Category does not exist!");
            }
        }

        public IQueryable<Category> Container()
        { // Returns the list of categories as IQueryable Category
            return categories.AsQueryable();
        }

        public void Delete(string Id)
        { // Finds the Category using its Id then
          // once category is found then its reference is stored
          // in categoryToDelete and if found not null then 
          // that it is removed from the list of categories

            Category categoryToDelete = categories.Find(c => c.Id == Id);

            if (categoryToDelete != null)
            {
                categories.Remove(categoryToDelete);
            }
            else
            {
                throw new Exception("Category does not exist!");
            }
        }
    }
}
