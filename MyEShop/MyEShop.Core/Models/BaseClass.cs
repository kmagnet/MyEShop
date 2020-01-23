using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public abstract class BaseClass
    {
        public string Id { get; set;  } // Introduce the Id variable to the inheriting class 'InMemoryContainer'
        public DateTimeOffset CreatedAt { get; set; } // Variable to contain the time when classes are created

        public BaseClass() {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
