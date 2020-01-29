using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using MyEShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyEShop.Services
{
    public class BasketService : IBasketService
    {
        IContainer<Product> ProductContext;
        IContainer<Basket> BasketContext;

        public const string BasketSessionName = "eShopBasket";

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

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext) {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                               join p in ProductContext.Container() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   id = b.Id,
                                   Quantity = b.Quantity,
                                   ProductName = p.Name,
                                   Image = p.Image,
                                   Price = p.Price
                               }).ToList();

                return results;
            }
            else {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext) {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                // Store the number of items in the basket into basketCount otherwise store null value
                int? basketCount = (from item in basket.BasketItems select item.Quantity).Sum();

                // Store the total price of the items found in the basket otherwise store  null value
                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in ProductContext.Container() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0; // If basketCount has a value then store that 
                                                      // value into the model BasketCount otherwise
                                                      // if basketCount is null then default to 0
                
                model.BasketTotal = basketTotal ?? decimal.Zero; // As above but if null then store decimal zero for data type accuracy

                return model;
            }
            else {
                return model;
            }
        }
    }
}
