using MembershipWebsite.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MembershipWebsite.Extensions
{
    public static class SubscriptionExtensions
    {
        public static async Task<int> GetSubscriptionIdByRegistrationCode(
            this IDbSet<Subscription> subscription, string code)
        {
            try
            {
                if (subscription == null || code == null || code.Equals(string.Empty))
                    return Int32.MinValue;

                var subscriptionId = await (
                    from s in subscription
                    where s.RegistrationCode.Equals(code)
                    select s.Id).FirstOrDefaultAsync();

                return subscriptionId;
            }
            catch { return Int32.MinValue; }
    }
}