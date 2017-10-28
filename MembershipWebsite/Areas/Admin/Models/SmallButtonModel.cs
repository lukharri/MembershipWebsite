using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MembershipWebsite.Areas.Admin.Models
{
    public class SmallButtonModel
    {
        public string Action { get; set; }

        // browser optimization
        public string Text { get; set; }

        // hold the name of the glyph on the button
        public string Glyph { get; set; }

        // button type - success, danger, etc...
        public string ButtonType { get; set; }

        // id properties to be given to Url
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public int? ProductId { get; set; }
        public int? SubscriptionId { get; set; }

        // how to combine IDs into the Url
        public string ActionParameters {
            // return the string built from using the IDs
            get {
                var param = new StringBuilder("?");
                if (Id != null && Id > 0)
                    param.Append(String.Format("{0}={1}&", "id", Id));

                if (ItemId != null && ItemId > 0)
                    param.Append(String.Format("{0}={1}&", "itemId", ItemId));

                if (ProductId != null && ProductId > 0)
                    param.Append(String.Format("{0}={1}&", "productId", ProductId));

                if (SubscriptionId != null && SubscriptionId > 0)
                    param.Append(String.Format("{0}={1}&", "subscriptionId", SubscriptionId));

                return param.ToString().Substring(0, param.Length - 1);
            }
        }
    }
}