using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public class Product : BaseClass
    {
        // Product class inherits from BaseClass the Id attribute and constructor

        [StringLength(20)] // Model validation using ComponentModel and DataAnnotations namespaces to limit product name to max of 20 characters
        [DisplayName("Product Name")] // Controls how this property will be rendered on the View
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0,1000)] // Property decorator to set the range of values for price from 0 to 1000
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
                
    }
}
