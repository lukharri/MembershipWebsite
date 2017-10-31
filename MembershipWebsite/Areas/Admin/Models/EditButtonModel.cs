using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MembershipWebsite.Areas.Admin.Models
{
    public class EditButtonModel
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }
        public string Link {
            get
            {
                var sb = new StringBuilder("?");
                if (ItemId > 0) sb.Append(String.Format("{0}={1}&", "itemId", ItemId));
                if (ProductId > 0) sb.Append(String.Format("{0}={1}&", "productId", ProductId));
                if (SubscriptionId > 0) sb.Append(String.Format("{0}={1}&", "subscriptionId", SubscriptionId));
                return sb.ToString().Substring(0, sb.Length - 1);
            }
        }    
    }
}