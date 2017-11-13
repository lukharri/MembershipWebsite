using MembershipWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MembershipWebsite.Controllers
{
    // Reachable only by users who have logged in
    [Authorize]
    public class ProductContentController : Controller
    {
        // GET: ProductContent
        public async Task<ActionResult> Index(int id)
        {
            var model = new ProductSectionModel
            {
                Title = "the title",
                Sections = new List<ProductSection>()
            };
            return View(model);
        }
    }
}