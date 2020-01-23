using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.DataAccess.InMemory
{
    public class InMemoryContainer<placeholder> : IContainer<placeholder> where placeholder : BaseClass
    {
        // Generic class definition where the object type 
        // being passed must be of type BaseClass or inherit from BaseClass

        ObjectCache cache = MemoryCache.Default;
        List<placeholder> items;
        string className;

        public InMemoryContainer()
        {

            className = typeof(placeholder).Name; // Takes the name of the class that is in the placeholder object ie Product, Category, etc.
            items = cache[className] as List<placeholder>; // Initialize the items variable with the values found in the memory cache 
                                                           // as a list of the placeholder's object type

            if (items == null)
            {
                items = new List<placeholder>(); // if items object is null then create a new instance of list of the placeholder's object type
            }

        }

        public void Confirm() { cache[className] = items; } // Store the list into the cache 

        public void Insert(placeholder p) { items.Add(p); } // Insert an object of placeholder type into the items list

        public void Update(placeholder p)
        {

            placeholder pToUpdate = items.Find(x => x.Id == p.Id);

            if (pToUpdate != null)
            {
                pToUpdate = p;
            }
            else
            {
                throw new Exception(className + " does not exist!");
            }
        }

        public placeholder Search(string Id)
        {
            placeholder p = items.Find(x => x.Id == Id);

            if (p != null)
            {
                return p;
            }
            else
            {
                throw new Exception(className + " does not exist!");
            }
        }

        public IQueryable<placeholder> Container() { return items.AsQueryable(); }

        public void Delete(string Id)
        {
            placeholder pToDelete = items.Find(x => x.Id == Id);

            if (pToDelete != null)
            {
                items.Remove(pToDelete);
            }
            else
            {
                throw new Exception(className + " does not exist!");
            }
        }
    }
}
