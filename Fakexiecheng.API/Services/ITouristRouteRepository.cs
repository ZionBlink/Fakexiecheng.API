using Fakexiecheng.API.helper;
using Fakexiecheng.API.Moldes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fakexiecheng.API.Services
{
   public interface ITouristRouteRepository
    {
       Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(
           string keyword, 
           string ratingOperator ,
           int? ratingValue,
           int pageSize,
           int pageNumber,
           string orderBy

           
           
           );

        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);


        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);

        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);

        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRoute,  TouristRoutePicture touristRoutePicture);

        void DeleteTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoutes(Task<IEnumerable<TouristRoute>> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture picture);

        Task<ShoppingCart> GetShoppingCartByUserId(string userId);
        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);
        Task<LineItem> GetShoppingCartItemByItemId(int LineItemId);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids);
        void DeleteShoppingCarItems(IEnumerable<LineItem> lineItems);

        Task AddOrderAsync(Order order);

        Task<PaginationList<Order>> GetOrdersByUserId(string userId, int pageSize, int pageNumber);


       Task<Order> GetOrderById(Guid orderID);

        Task<bool> SaveAsync();
        
    }
}
