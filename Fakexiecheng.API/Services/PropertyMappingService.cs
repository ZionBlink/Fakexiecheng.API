﻿using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Moldes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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


        public bool IsMappingExists<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource,TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {   //去空格
                
                var trimmedField = field.Trim();
                //获得属性名称字符串
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                        trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                
                
                }

            }
            return true;
        }

        public bool IsPropertiesExists<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public |  BindingFlags .Instance);

                if (propertyInfo ==null) {

                    return false;
                }

            }
            return true;
        
        }
    }
}
