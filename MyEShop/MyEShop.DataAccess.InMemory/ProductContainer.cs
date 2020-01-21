using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyEShop.Core.Models;

namespace MyEShop.DataAccess.InMemory
{
    public class ProductContainer
    {
        ObjectCache cache = MemoryCache.Default; // Creates an ObjectCache containing the reference for the default MemoryCache instance which 
                                                 // serves as flexible temporary storage for data that my application can process
        List<Product> products;

        public ProductContainer() 
        {
            products = cache["products"] as List<Product>; // When the constructor is launched, it will look into the cache to see if there
                                                           // is a cache called products which is a list of products but if that returns null
                                                           // then it creates a new list of products
            if (products == null) {
                products = new List<Product>();
            }
        }

        public void Confirm() {
            cache["products"] = products; // When people add products, we don't want them to save it straight away
                                          // so they will get stored first as a list before we are finally ready to move 
                                          // them into the cache then into the SQL database.
        }

        public void Insert(Product product) { // Simply adds a product into the list products
            products.Add(product);
        }

        public void Update(Product product) { // Finds the product to be updated via its Id
                                              // then stores its reference then once product is found
                                              // it then pushes the updates into the original product instance

            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else {
                throw new Exception("Product does not exist!");
            }
        }

        public Product Search(string Id) { // Searches for a product by Id then returns the
                                           // object reference for the product requested

            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product does not exist!");
            }
        }

        public IQueryable<Product> Container() { // Returns the list of products as IQueryable Product
            return products.AsQueryable();
        }

        public void Delete(string Id) { // Finds the product using its Id then
                                        // once product is found then its reference is store
                                        // in productToDelete and it found not null then 
                                        // that is removed from the list of products

            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product does not exist!");
            }
        }
    }
}
