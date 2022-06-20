using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fakexiecheng.API.Database;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.helper;
using Fakexiecheng.API.Moldes;
using Microsoft.EntityFrameworkCore;

namespace Fakexiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {

        private readonly AppDbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public TouristRouteRepository(AppDbContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public  async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(
            string keyword,
             string ratingOperator,
            int? ratingValue,
            int pageSize,
            int pageNumber,
            string orderBy
            )
        {
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t=>t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword)) {
                keyword = keyword.Trim();
                result = result.Where(t=> t.Title.Contains(keyword));
            }
            if (ratingValue >= 0) {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
               /* if (orderBy.ToLowerInvariant()=="originalprice")//ToLowerInvariant仅限英语效率高
                {
                    result = result.OrderBy(t => t.OriginalPrice);
                }*/
                var touristRouteMappingDictionary = _propertyMappingService
                    .GetPropertyMapping<TouristRouteDto,TouristRoute>();
               result= result.ApplySort(orderBy, touristRouteMappingDictionary);

            }


            return await PaginationList<TouristRoute>.CreateAsync(pageNumber, pageSize, result);
        }
        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return  await  _context.TouristRoutes.AnyAsync(n => n.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures.Where(P => P.TouristRouteId ==touristRouteId).ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId) 
        {
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }



        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null) {

                throw new ArgumentNullException(nameof(touristRoute));
            }

            _context.TouristRoutes.Add(touristRoute);
            _context.SaveChanges();

        }
        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture) 
        
        
        {
            if (touristRouteId == Guid.Empty) {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null) {

                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);

        }
        public void DeleteTouristRoute(TouristRoute touristRoute) {

            _context.TouristRoutes.Remove(touristRoute);
        }
        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {

            _context.TouristRoutePictures.Remove(picture);
        }

        public  async Task<bool> SaveAsync() {

            return  (await _context.SaveChangesAsync()>=0);
        }

       

       
       

       public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public void DeleteTouristRoutes(Task<IEnumerable<TouristRoute>> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange((IEnumerable<TouristRoute>)touristRoutes);
        }

        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User).Include(s => s.ShoppingCartItems)
                .ThenInclude(li => li.TouristRoute).Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
              await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems.Where(li => li.Id == lineItemId).FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids)
        {
            return await _context.LineItems.Where(li => ids.Contains(li.Id)).ToListAsync();
        }

        public void DeleteShoppingCarItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<PaginationList<Order>> GetOrdersByUserId(string userId ,int pageSize ,int pageNumber)
        {
            // return  await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            IQueryable<Order> result = _context.Orders.Where(o => o.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
             
        }

        public async Task<Order> GetOrderById(Guid orderID)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.TouristRoute)
                .Where(o => o.Id == orderID).FirstOrDefaultAsync();
        }
    }
}
