using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;

namespace MembershipWebsite.Extensions
{
    public static class HttpContextExtensions
    {
        // contains the path to the name identifier claim
        private const string nameidentifier =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        // To get to user info you go through the Owin context on the current httpContext object
        // user id will be used when fetching subscriptions for the logged in user
        // The Owin context works with claims when handling identities
        // The claim we are interested in for the user Id is the name identifier 
        public static string GetUserId(this HttpContextBase ctx)
        {
            var uid = string.Empty;
            try
            {
                var claims = ctx.GetOwinContext().Get<ApplicationSignInManager>()
                    .AuthenticationManager.User.Claims.FirstOrDefault(claim =>
                    claim.Type.Equals(nameidentifier));

                if (claims != default(Claim))
                    uid = claims.Value;
            }
            catch
            {

            }
            return uid;
        }
    }
}