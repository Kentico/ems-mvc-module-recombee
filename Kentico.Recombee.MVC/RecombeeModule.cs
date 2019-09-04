﻿using System.Linq;

using CMS;
using CMS.Activities;
using CMS.Base;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;

using Kentico.Recombee;


[assembly: RegisterModule(typeof(RecombeeModule))]

namespace Kentico.Recombee
{
    public class RecombeeModule : Module
    {
        public RecombeeModule() : base("RecombeeModule", false)
        {
        }


        protected override void OnInit()
        {
            base.OnInit();
            EcommerceEvents.ProductAddedToShoppingCart.Execute += ProductAddedToShoppingCart_Execute;
            EcommerceEvents.NewOrderCreated.Execute += NewOrderCreated_Execute;
            ActivityEvents.ActivityProcessedInLogService.Execute += ActivityProcessedInLogService_Execute;

        }


        private void ActivityProcessedInLogService_Execute(object sender, CMSEventArgs<IActivityInfo> e)
        {
            var activity = e.Parameter;

            if (activity.ActivityType == PredefinedActivityType.PAGE_VISIT)
            {
                var activityContact = ContactInfoProvider.GetContactInfo(activity.ActivityContactID);
                if (activityContact != null)
                {

                }
            }
        }


        private void NewOrderCreated_Execute(object sender, NewOrderCreatedEventArgs e)
        {
            var contact = ContactManagementContext.GetCurrentContact();
            var service = new RecombeeClientService();
            var order = e.NewOrder;
            var orderItems = OrderItemInfoProvider.GetOrderItems(order.OrderID);

            foreach (var orderItem in orderItems)
            {
                var product = GetProductForSKU(orderItem.OrderItemSKUID);
                service.LogProductPurchase(contact.ContactGUID, product.DocumentGUID, orderItem.OrderItemUnitCount);
            }
        }


        private void ProductAddedToShoppingCart_Execute(object sender, ProductAddedToCartEventArgs e)
        {
            var contact = ContactManagementContext.GetCurrentContact();
            var service = new RecombeeClientService();

            var skuId = e.AddedShoppingCartItem.SKU.SKUID;
            var product = GetProductForSKU(skuId);

            service.LogProductAddition(contact.ContactGUID, product.DocumentGUID, e.AddedShoppingCartItem.CartItemUnits);
        }


        private SKUTreeNode GetProductForSKU(int skuID)
        {
            var sku = SKUInfoProvider.GetSKUInfo(skuID);
            if ((sku == null) || sku.IsProductOption)
            {
                return null;
            }

            if (sku.IsProductVariant)
            {
                skuID = sku.SKUParentSKUID;
            }

            var node = DocumentHelper.GetDocuments()
                .Published()
                .OnSite(sku.SKUSiteID)
                .CombineWithDefaultCulture()
                .WhereEquals("NodeSKUID", skuID)
                .FirstOrDefault();

            return node as SKUTreeNode;
        }
    }
}