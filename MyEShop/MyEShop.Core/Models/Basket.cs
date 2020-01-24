using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public class Basket: BaseClass
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; } //

        public Basket() {
            this.BasketItems = new List<BasketItem>();
        }
    }
}
