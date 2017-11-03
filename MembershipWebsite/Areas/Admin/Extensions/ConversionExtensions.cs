using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MembershipWebsite.Areas.Admin.Models;
using MembershipWebsite.Entities;
using MembershipWebsite.Models;
using System.Data.Entity;
using System.Transactions;

namespace MembershipWebsite.Areas.Admin.Extensions
{
    public static class ConversionExtensions
    {
        #region Product
        public static async Task<IEnumerable<ProductModel>> Convert(
            this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products.Count().Equals(0))
                return new List<ProductModel>();
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeId,
                       ProductLinkTexts = texts,
                       ProductTypes = types
                   };
        }


        // Convert 1 product into 1 productModel
        public static async Task<ProductModel> Convert(
        this Product product, ApplicationDbContext db)
        {
            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductLinkTextId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductTypeId));

            var model =  new ProductModel
                   {
                       Id = product.Id,
                       Title = product.Title,
                       Description = product.Description,
                       ImageUrl = product.ImageUrl,
                       ProductLinkTextId = product.ProductLinkTextId,
                       ProductTypeId = product.ProductTypeId,
                       ProductLinkTexts = new List<ProductLinkText>(),
                       ProductTypes = new List<ProductType>()
                   };

            model.ProductLinkTexts.Add(text);
            model.ProductTypes.Add(type);

            return model;
        }
        #endregion

        #region Product Item
        // Convert ProductItem to ProductItemModel
        public static async Task<IEnumerable<ProductItemModel>> Convert(
        this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            if (productItems.Count().Equals(0))
                return new List<ProductItemModel>();

            return await (from pi in productItems
                   select new ProductItemModel
                   {
                       ItemId = pi.ItemId,
                       ProductId = pi.ProductId,
                       ItemTitle = db.Items.FirstOrDefault(
                           i => i.Id.Equals(pi.ItemId)).Title,
                       ProductTitle = db.Products.FirstOrDefault(
                           i => i.Id.Equals(pi.ProductId)).Title

                   }).ToListAsync();
        }


        // Convert 1 ProductItem into 1 productItemModel
        // Enables the ProductItem edit view to render 1 productItem as a productIemModel
        public static async Task<ProductItemModel> Convert(
        this ProductItem productItem, ApplicationDbContext db,
        bool addListData = true)
        {
            var model = new ProductItemModel
            {
                ItemId = productItem.ItemId,
                ProductId = productItem.ProductId,
                Items = addListData ? await db.Items.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                ItemTitle = (await db.Items.FirstOrDefaultAsync(i => 
                    i.Id.Equals(productItem.ItemId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p => 
                    p.Id.Equals(productItem.ProductId))).Title
            };

            return model;
        }

        // Check if there is a productItem in the database w/the product id and item id combo
        // from the old product id and old item id values from the view - this is the productItem
        // to be changed/removed
        public static async Task<bool> CanChange(
            this ProductItem productItem, ApplicationDbContext db)
        {
            var oldPI = await db.ProductItems.CountAsync(pi =>
            pi.ProductId.Equals(productItem.OldProductId) &&
            pi.ItemId.Equals(productItem.OldItemId));

            var newPI = await db.ProductItems.CountAsync(pi =>
            pi.ProductId.Equals(productItem.ProductId) &&
            pi.ItemId.Equals(productItem.ItemId));

            // Return true if original values exist in the db and the new values do not
            return oldPI.Equals(1) && newPI.Equals(0);
        }


        // Replace an existing value pair in the productItem table w/new value pair
        // selected by user from drop-downs in the edit view
        // Result stored in db if successful, otherwise it is rolled back
        public static async Task Change(this ProductItem productItem, ApplicationDbContext db)
        {
            // fetch the productItem whose original product and item IDs
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(
                pi => pi.ProductId.Equals(productItem.OldProductId) &&
                pi.ItemId.Equals(productItem.OldItemId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(
                pi => pi.ProductId.Equals(productItem.ProductId) &&
                pi.ItemId.Equals(productItem.ItemId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemId = productItem.ItemId,
                    ProductId = productItem.ProductId
                };

                using (var transaction = new TransactionScope(
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);
                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }
            }
        }
        #endregion


        #region Subscription Product
        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(
        this IQueryable<SubscriptionProduct> subscriptionProducts, ApplicationDbContext db)
        {
            if (subscriptionProducts.Count().Equals(0))
                return new List<SubscriptionProductModel>();

            return await (from pi in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              SubscriptionId = pi.SubscriptionId,
                              ProductId = pi.ProductId,
                              SubscriptionTitle = db.Subscriptions.FirstOrDefault(
                                  i => i.Id.Equals(pi.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(
                                  p => p.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
        }


        public static async Task<SubscriptionProductModel> Convert(
        this SubscriptionProduct subscriptionProduct,
        ApplicationDbContext db, bool addListData = true)
        {
            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionProduct.SubscriptionId,
                ProductId = subscriptionProduct.ProductId,
                Subscriptions = addListData ? await db.Subscriptions.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                SubscriptionTitle = (await db.Subscriptions.FirstOrDefaultAsync(s =>
                   s.Id.Equals(subscriptionProduct.SubscriptionId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p =>
                   p.Id.Equals(subscriptionProduct.ProductId))).Title
            };

            return model;
        }


        public static async Task<bool> CanChange(
            this SubscriptionProduct subscriptionProduct,
            ApplicationDbContext db)
        {
            var oldSP = await db.SubscriptionProducts.CountAsync(sp =>
                sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSP = await db.SubscriptionProducts.CountAsync(sp =>
                sp.ProductId.Equals(subscriptionProduct.ProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return oldSP.Equals(1) && newSP.Equals(0);
        }


        public static async Task Change(
            this SubscriptionProduct subscriptionProduct,
            ApplicationDbContext db)
        {
            var oldSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (oldSubscriptionProduct != null && newSubscriptionProduct == null)
            {
                newSubscriptionProduct = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionProduct.SubscriptionId,
                    ProductId = subscriptionProduct.ProductId
                };

                using (var transaction = new TransactionScope(
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.SubscriptionProducts.Remove(oldSubscriptionProduct);
                        db.SubscriptionProducts.Add(newSubscriptionProduct);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }
            }
        }
        #endregion
    }
}