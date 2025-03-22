﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ePizza.UI.Helpers
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            tempData[key] = JsonSerializer.Serialize(value, options);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out object o);
            return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o = tempData.Peek(key);
            return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        }
    }
}
