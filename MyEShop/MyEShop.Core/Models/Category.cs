using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public class Category // This is the model for the product category
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }

        public Category() { // This is the constructor for product categories
                            // which only creates a new ID for the category upon object instantiation.

            this.Id = Guid.NewGuid().ToString();

        }
    }
}
