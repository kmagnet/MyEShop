using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public class Category : BaseClass 
    {
        // This is the model for the product category which
        // inherits the Id attribute from BaseClass along with the constructor
                
        public string CategoryName { get; set; }
               
    }
}
