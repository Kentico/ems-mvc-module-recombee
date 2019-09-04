using System;

using CMS.Ecommerce;

namespace Kentico.Recombee.Models.Widgets.RecommendedProducts
{
    /// <summary>
    /// View model for an item in Recommended products widget.
    /// </summary>
    public class RecommendedProductViewModel
    {
        public ProductCatalogPrices PriceDetail { get; }


        public string Name { get; }


        public string ImagePath { get; }


        public string PublicStatusName { get; }


        public bool Available { get; }


        public Guid ProductPageGUID { get; }


        public string ProductPageAlias { get; }


        /// <summary>
        /// Creates an instance of the <see cref="RecommendedProductViewModel"/> view model-
        /// </summary>
        /// <param name="productPage">Product page.</param>
        /// <param name="priceDetail">Price.</param>
        public RecommendedProductViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail)
        {
            // Set page information
            Name = productPage.DocumentName;
            ProductPageGUID = productPage.NodeGUID;
            ProductPageAlias = productPage.NodeAlias;

            // Set SKU information
            ImagePath = productPage.SKU.SKUImagePath;
            Available = !productPage.SKU.SKUSellOnlyAvailable || productPage.SKU.SKUAvailableItems > 0;

            // Set additional info
            PriceDetail = priceDetail;
        }
    }
}