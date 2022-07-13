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
using Fakexiecheng.API.ResoureceParameters;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fakexiecheng.API.controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController:ControllerBase



    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;     

        public OrdersController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper,  IHttpClientFactory     httpClientFactory  )
        {

            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }


        [HttpGet(Name = "GetOrders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] PaginationResourceParamaters paginationResourceParamaters
            ) 
        
        
        {
            //1
            var userId = _httpContextAccessor
               .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //2
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId, paginationResourceParamaters.PageSize, paginationResourceParamaters.PageNumber);

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

        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder([FromRoute]Guid orderId)
        {
            //1.
            var userId = _httpContextAccessor
                 .HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //2.
            var order = await _touristRouteRepository.GetOrderById(orderId);
            order.PaymentProcessing();
            await _touristRouteRepository.SaveAsync();
            //3.
            var httpClient = _httpClientFactory.CreateClient();
            string url = @"";//
            var response= await httpClient.PostAsync(
                string.Format(url, "", order.Id, false), null);
            //4.
            bool isApproved = false;
            string transactionMetadate = "";
            if (response.IsSuccessStatusCode) {
                transactionMetadate = await response.Content.ReadAsStringAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(transactionMetadate);
                isApproved = jsonObject["approved"].Value<bool>();

            
            }


            if (isApproved)
            {
                order.PaymentApprove();

            }
            else 
            {
                order.PaymentReject();
            
            }
            order.TransactionMetadata = transactionMetadate;
            await _touristRouteRepository.SaveAsync();
            return Ok(_mapper.Map<OrderDto>(order));
        }

    }
}
