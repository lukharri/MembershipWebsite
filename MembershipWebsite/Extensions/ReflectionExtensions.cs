using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipWebsite.Extensions
{
    public static class ReflectionExtensions
    {
        /*
         *   Extension methods must be declared as public static and reside in a class of the same
         *   Called from the toSelectList extension method
         *      Fetches property values w/reflection from individual items in the collection
         *      passed into the toSelectList item method
         *   Params
         *      generic type T represents the item to reflect over to read its property values
         *      string for the name of the properties we want to fetch
         *   Returns a string representing the property value
         *   First param in an extension method has to be preceded by the 'this' keyword to make it
         *      possible to call the method on any object w/a data type defined by this parameter
         *   The generic type T makes it possible to use it on any type
         */
        public static string GetPropertyValue<T>(
            this T item, string propertyName)
        {
            return item.GetType()
                .GetProperty(propertyName)
                .GetValue(item, null).ToString();
        }
    }
}