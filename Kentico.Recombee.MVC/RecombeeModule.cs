using System.Linq;

using CMS;
using CMS.Activities;
using CMS.Base;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;

using Kentico.Recombee;

[assembly: RegisterModule(typeof(RecombeeModule))]

namespace Kentico.Recombee
{
    /// <summary>
    /// Recombee module for MVC instance.
    /// </summary>
    public class RecombeeModule : Module
    {
        /// <summary>
        /// Creates an instance of the <see cref="RecombeeModule"/> class.
        /// </summary>
        public RecombeeModule() : base("RecombeeClientModule", false)
        {
        }


        protected override void OnInit()
        {
            base.OnInit();
            EcommerceEvents.ProductAddedToShoppingCart.Execute += ProductAddedToShoppingCart_Execute;
            EcommerceEvents.NewOrderCreated.Execute += NewOrderCreated_Execute;
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
