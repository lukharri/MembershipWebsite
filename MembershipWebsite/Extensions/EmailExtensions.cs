using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MembershipWebsite.Extensions
{
    public static class EmailExtensions
    {
        public static void Send(this IdentityMessage message)
        {

            try
            {
                // fetch password and host from webconfig
                var password = ConfigurationManager.AppSettings["password"];
                var from = ConfigurationManager.AppSettings["from"];
                var host = ConfigurationManager.AppSettings["host"];
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

                // create email to send
                var email = new MailMessage(from, message.Destination, message.Subject, message.Body);
                email.IsBodyHtml = true;

                // create an SMTP client to send the email
                var client = new SmtpClient(host, port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from, password);

                // send the email
                client.Send(email);
            }
            catch 
            {
            }
        }
    }
}