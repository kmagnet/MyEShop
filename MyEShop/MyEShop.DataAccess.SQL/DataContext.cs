using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.DataAccess.SQL
{
    public class DataContext :DbContext
    {
        public DataContext()
            : base("DefaultCOnnection") { 

            /// Tells DataContext class to look into the Web.config (or into App.config) file and look for section called DefaultConnection
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

    }
}
