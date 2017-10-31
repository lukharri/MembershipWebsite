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
        this ProductItem productItem, ApplicationDbContext db)
        {
            var model = new ProductItemModel
            {
                ItemId = productItem.ItemId,
                ProductId = productItem.ProductId,
                Items = await db.Items.ToListAsync(),
                Products = await db.Products.ToListAsync()
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
    }
}