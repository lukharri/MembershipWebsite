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


        // Registers a subscription id matching the code entered to the logged in user
        // by adding a new entry in the UserSubscription table
        public static async Task Register(
            this IDbSet<UserSubscription> userSubscription, int subscriptionId, string userId)
        {
            try
            {
                if (userSubscription == null ||
                    subscriptionId.Equals(Int32.MinValue) ||
                    userId.Equals(string.Empty))
                    return;

                var exists = await Task.Run(() => userSubscription.CountAsync(
                    s => s.SubscriptionId.Equals(subscriptionId) &&
                    s.UserId.Equals(userId))) > 0;

                if (!exists)
                    await Task.Run(() => userSubscription.Add(
                        new UserSubscription
                        {
                            UserId = userId,
                            SubscriptionId = subscriptionId,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.MaxValue
                        }));
            }
            catch { }
        }
    }
}