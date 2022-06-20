﻿using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Moldes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fakexiecheng.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _touristRoutePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {

                {"Id", new PropertyMappingValue(new List<string>(){"Id" }) },
                {"Title", new PropertyMappingValue(new List<string>(){"Title" }) },
                {"Rating", new PropertyMappingValue(new List<string>(){"Rating" }) },
                {"OriginalPrice", new PropertyMappingValue(new List<string>(){"OriginalPrice" }) },

            };
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(
                new PropertyMapping<TouristRouteDto, TouristRoute>(_touristRoutePropertyMapping)

                );



        }





        public Dictionary<string, PropertyMappingValue>
            GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }
            throw new Exception("can");

        }

    }
}
