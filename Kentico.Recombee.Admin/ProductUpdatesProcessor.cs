using System;
using CMS.Ecommerce;

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
            this.recombeeClientService = recombeeClientService ?? throw new ArgumentNullException(nameof(recombeeClientService));
        }


        /// <summary>
        /// Process added product in Recombee.
        /// </summary>
        /// <param name="productPage">Product page.</param>
        public void AddProduct(SKUTreeNode productPage)
        {
            if (productPage is null)
            {
                throw new ArgumentNullException(nameof(productPage));
            }

            recombeeClientService.UpdateProduct(productPage);
        }


        /// <summary>
        /// Deletes given product from recombee database.
        /// </summary>
        /// <param name="user">Product to delete.</param>
        public void DeleteProduct(SKUTreeNode productPage)
        {
            if (productPage is null)
            {
                throw new ArgumentNullException(nameof(productPage));
            }

            recombeeClientService.Delete(productPage);
        }
    }
}
