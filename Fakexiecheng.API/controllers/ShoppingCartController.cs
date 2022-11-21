using AutoMapper;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Moldes;
using Fakexiecheng.API.Services;
using FakeXiecheng.API.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fakexiecheng.API.controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {

            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;

        }


        [HttpGet(Name = "GetShoppingCart")]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()

        {
            //1 获得当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //2使用用户id获得购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserId(userId);



            //
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }
        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem(
            [FromBody] AddShoppingCartItemDto addShoppingCartItemDto
        )
        {
            // 1 获得当前用户
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2 使用userid获得购物车
            var shoppingCart = await _touristRouteRepository
                .GetShoppingCartByUserId(userId);

            // 3 创建lineItem
            var touristRoute = await _touristRouteRepository
                .GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if (touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            // 4 添加lineitem，并保存数据库
            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }
        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCarItem([FromRoute] int itemId)
        {

            //1
            var lineItem = await _touristRouteRepository.GetShoppingCartItemByItemId(itemId);
            if (lineItem == null)
            {
                return NotFound("购物车商品找不到");

            }
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();
            return NoContent();

        }
        [HttpDelete("items/({itemIDs})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveShoppingCartItems([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIDs)

        {
            var lineItems = await _touristRouteRepository.GetShoppingCartsByIdListAsync(itemIDs);

            _touristRouteRepository.DeleteShoppingCarItems(lineItems);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }


        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> Checkout()

        {
            // 1 获得当前用户
            var userId = _httpContextAccessor
                .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2 使用userid获得购物车
            var shoppingCart = await _touristRouteRepository
                .GetShoppingCartByUserId(userId);
            //3 创建订单
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart.ShoppingCartItems,
                CreateDateUTC = DateTime.UtcNow,


            };
            shoppingCart.ShoppingCartItems = null;
            //4 保存数据
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();
            //
            return Ok(_mapper.Map<OrderDto>(order));

        }




    }
}
