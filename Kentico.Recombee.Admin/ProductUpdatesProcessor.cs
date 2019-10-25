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
            this.recombeeClientService = recombeeClientService;
        }


        /// <summary>
        /// Process added product in Recombee.
        /// </summary>
        /// <param name="productPage">Product page.</param>
        public void ProcessNewProduct(SKUTreeNode productPage)
        {
            recombeeClientService.UpdateProduct(productPage);
        }
    }
}
