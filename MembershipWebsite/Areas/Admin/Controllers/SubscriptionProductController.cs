﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MembershipWebsite.Entities;
using MembershipWebsite.Models;

namespace MembershipWebsite.Areas.Admin.Controllers
{
    public class SubscriptionProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SubscriptionProduct
        public async Task<ActionResult> Index()
        {
            return View(await db.Subscriptions.ToListAsync());
        }

        // GET: Admin/SubscriptionProduct/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // GET: Admin/SubscriptionProduct/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SubscriptionProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,RegistrationCode")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                db.Subscriptions.Add(subscription);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subscription);
        }

        // GET: Admin/SubscriptionProduct/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // POST: Admin/SubscriptionProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,RegistrationCode")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscription).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subscription);
        }

        // GET: Admin/SubscriptionProduct/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // POST: Admin/SubscriptionProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Subscription subscription = await db.Subscriptions.FindAsync(id);
            db.Subscriptions.Remove(subscription);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private async Task<SubscriptionProduct> GetSubscriptionProduct(int? subscriptionId, int? productId)
        {
            try
            {
                // Convert nullable int params to int values
                int subscrId = 0, prdId = 0;
                int.TryParse(subscriptionId.ToString(), out subscrId);
                int.TryParse(productId.ToString(), out prdId);

                // Fetch the first product item w/matching product and item IDs
                var subscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(
                    pi => pi.ProductId.Equals(prdId) && pi.SubscriptionId.Equals(subscrId));

                return subscriptionProduct;
            }

            catch { return null; }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
