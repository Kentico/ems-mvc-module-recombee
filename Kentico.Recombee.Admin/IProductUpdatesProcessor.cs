using CMS;
using CMS.Ecommerce;

using Kentico.Recombee;

[assembly: RegisterImplementation(typeof(IProductUpdatesProcessor), typeof(ProductUpdatesProcessor))]

namespace Kentico.Recombee
{
    /// <summary>
    /// The contract for an implementation handling product updates.
    /// </summary>
    public interface IProductUpdatesProcessor
    {
        /// <summary>
        /// Process added product in Recombee.
        /// </summary>
        /// <param name="productPage">Product page.</param>
        void AddProduct(SKUTreeNode productPage);


        /// <summary>
        /// Deletes given product from recombee database.
        /// </summary>
        /// <param name="user">Product to delete.</param>
        void DeleteProduct(SKUTreeNode productPage);


        /// <summary>
        /// Updates given product from recombee database.
        /// </summary>
        /// <param name="user">Product to update.</param>
        void UpdateProduct(SKUTreeNode productPage);
    }
}