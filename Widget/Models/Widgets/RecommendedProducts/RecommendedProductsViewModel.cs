using System.Collections.Generic;


namespace Kentico.Recombee.Models.Widgets.RecommendedProducts
{
    /// <summary>
    /// View model for Recommended products widget.
    /// </summary>
    public class RecommendedProductsViewModel
    {
        /// <summary>
        /// Collection of recommended products.
        /// </summary>
        public IEnumerable<RecommendedProductViewModel> Products;

        /// <summary>
        /// Creates an instace of the <see cref="RecommendedProductsViewModel"/> class.
        /// </summary>
        public RecommendedProductsViewModel()
        {
            Products = new List<RecommendedProductViewModel>();
        }
    }
}