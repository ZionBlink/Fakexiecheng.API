﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Fakexiecheng.API.helper
{
    public static class IEumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeDate<TSource>(
                this IEnumerable<TSource> source,
                string fields
            )
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var expandoObjectList = new List<ExpandoObject>();
            //避免遍历
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.IgnoreCase
                    | BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);



            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();


                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase
                         | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"属性{propertyName}找不到"+
                            $"{typeof(TSource)}");
                    
                    }

                    propertyInfoList.Add(propertyInfo);




                }



            
            }

            foreach (
                TSource sourceObject in source) 
            
            
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name,propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);

            }
            return expandoObjectList;
        }


    }
}
