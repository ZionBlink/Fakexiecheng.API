using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Fakexiecheng.API.Services;
using AutoMapper;

using Fakexiecheng.API.Dtos;

namespace Fakexiecheng.API.controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController:ControllerBase



    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public OrdersController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {

            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;

            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders() 
        
        
        {
            //1
            var userId = _httpContextAccessor
               .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //2
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute]Guid orderId ) 
        
        {
            var userId = _httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var order = await _touristRouteRepository.GetOrderById(orderId);
            return Ok(_mapper.Map<OrderDto>(order));


        }

    }
}
