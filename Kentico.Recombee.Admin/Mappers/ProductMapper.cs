using System;
using System.Collections.Generic;

using CMS.Ecommerce;

namespace Kentico.Recombee.Mappers
{
    /// <summary>
    /// Encapsulates mapping Kentico objects to Recombee parameters.
    /// </summary>
    public static class ProductMapper
    {
        /// <summary>
        /// Maps <paramref name="productPage"/> to Recombee structure.
        /// </summary>
        /// <param name="productPage">Product page.</param>
        public static Dictionary<string, object> Map(SKUTreeNode productPage)
        {
            if (productPage is null)
            {
                throw new ArgumentNullException(nameof(productPage));
            }

            return new Dictionary<string, object>
            {
                { "Name", productPage.DocumentSKUName },
                { "Description", productPage.DocumentSKUShortDescription },
                { "Content", productPage.DocumentSKUDescription},
                { "Culture", productPage.DocumentCulture},
                { "ClassName", productPage.ClassName},
                { "Price", productPage.SKU.SKUPrice },
                { "Type", "Product"},
            };
        }
    }
}
