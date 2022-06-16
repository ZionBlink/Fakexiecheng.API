/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fakexiecheng.API.Moldes;

namespace Fakexiecheng.API.Services
{
    public class MockTouristRouteRepository : ITouristRouteRepository
    {
        private List<TouristRoute> _routes;

        public MockTouristRouteRepository() {
            if (_routes == null) {
                InitializeTouristRoutes();
            }
        }

        private void InitializeTouristRoutes()
        {
            _routes = new List<TouristRoute> {
           new TouristRoute{ Id= Guid.NewGuid(),
           Title="黄山"
           }
           };
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _routes.FirstOrDefault(n => n.Id==touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes()
        {
            return _routes;
        }
    }
}
*/