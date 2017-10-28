using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipWebsite.Extensions
{
    public static class ICollectionExtensions
    {
        /*
         *  ToSelectListItem - convert a collection of type T into a collection of SelectLisItem 
         *  Used to display drop-downs in MVC views
         *  Params
         *      - generic collection of ICollection<T> which contain the items to reflect over to get
         *        their property values
         *      - int for id of selected item
         *  Returns a collection of type IEnumerable of SelectListItem
         */

        public static IEnumerable<SelectListItem>
            ToSelectListItem<T>(
            this ICollection<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Title"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id")
                       .Equals(selectedValue.ToString())
                   };
        }

    }
}