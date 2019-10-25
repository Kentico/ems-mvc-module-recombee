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
        void ProcessNewProduct(SKUTreeNode productPage);
    }
}