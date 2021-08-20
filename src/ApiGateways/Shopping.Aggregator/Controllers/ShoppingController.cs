using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController: ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name="GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping (string userName)
        {
            //get basket with user name
            var basket = await _basketService.GetBasket(userName);
            //integrate basket items and consume products with basket item productId member
            foreach (var item in basket.Items)
            {
                // map product releted members into basketitems dto with exended columns
                var product = await _catalogService.GetCatalog(item.ProductId);
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            // consume ordering micorservices in order to reiceve order list
            var orders = await _orderService.GetOrdersByUserName(userName);
            //return root shoppingModel dto class which including all response
            var shoppingModel = new ShoppingModel
            {
                BasketWithProduct = basket,
                Orders = orders,
                Username = userName
            };
            return shoppingModel;
        }
    }
}
