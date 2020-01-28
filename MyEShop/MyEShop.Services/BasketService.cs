using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyEShop.Services
{
    public class BasketService
    {
        IContainer<Product> ProductContext;
        IContainer<Basket> BasketContext;

        public const string BasketSessionName = "EShopBasket";

        public BasketService(IContainer<Product> productContext, IContainer<Basket> basketContext) {
            
            BasketContext = basketContext;
            ProductContext = productContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull) {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = BasketContext.Search(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else {
                if (createIfNull) {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext) {
            Basket basket = new Basket();
            BasketContext.Insert(basket);
            BasketContext.Confirm();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            BasketContext.Confirm();
        }

        public void RemoteFromBasket(HttpContextBase httpContext, string itemId) {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null) {
                basket.BasketItems.Remove(item);
                BasketContext.Confirm();
            }

        }
    }
}
