using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.Core.Models
{
    public abstract class BaseClass
    {
        public string Id { get; set;  } // Id to be used during object instantiation for either contexts ie product or category
                                        // as context does not necessarily know variable Id nor does it know which object type
                                        // it will handle upfront

        public DateTimeOffset CreatedAt { get; set; } // Variable to contain the time when classes are created

        public BaseClass() {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
