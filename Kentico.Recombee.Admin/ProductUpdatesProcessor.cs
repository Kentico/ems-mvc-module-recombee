using System;
using System.Collections.Generic;

using CMS.Core;
using CMS.Ecommerce;

using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee
{
    /// <summary>
    /// Reflects product actions in Recombee database.
    /// </summary>
    public class ProductUpdatesProcessor : IProductUpdatesProcessor
    {
        private readonly IRecombeeClientService recombeeClientService;

        /// <summary>
        /// Creates an instance of the <see cref="ProductUpdatesProcessor"/> class.
        /// </summary>
        /// <param name="recombeeClientService">Client service.</param>
        public ProductUpdatesProcessor(IRecombeeClientService recombeeClientService)
        {
            this.recombeeClientService = recombeeClientService;
        }


        /// <summary>
        /// Process added product in Recombee.
        /// </summary>
        /// <param name="productPage">Product page.</param>
        public void ProcessNewProduct(SKUTreeNode productPage)
        {
            var productsToPush = new SetItemValues(productPage.DocumentGUID.ToString(),
                new Dictionary<string, object>
                {
                    { "Name", productPage.DocumentSKUName },
                    { "Description", productPage.DocumentSKUShortDescription },
                    { "Content", productPage.DocumentSKUDescription},
                    { "Culture", productPage.DocumentCulture},
                    { "ClassName", productPage.ClassName},
                    { "Price", productPage.SKU.SKUPrice },
                    { "Type", "Product"},
                }, true);

            try
            {
                recombeeClientService.Send(productsToPush);
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("RecombeeAdminModule", "ON_PRODUCT_CREATED", ex);
            }
        }
    }
}
