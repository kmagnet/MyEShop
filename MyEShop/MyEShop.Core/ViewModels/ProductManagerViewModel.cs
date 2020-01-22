using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.ViewModels
{
    public class ProductManagerViewModel // This class will contain data for temporary storage
    {                             // but main its purpose is to transport data between controller and view
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
