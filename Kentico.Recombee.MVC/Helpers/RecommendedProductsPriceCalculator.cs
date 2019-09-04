using System.Linq;
using CMS.Ecommerce;

namespace Kentico.Recombee.Helpers
{
    /// <summary>
    /// Provides prices calculations methods for recommended products.
    /// </summary>
    public class RecommendedProductsPriceCalculator
    {
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory;


        /// <summary>
        /// Creates an instace of the <see cref="RecommendedProductsPriceCalculator"/> class.
        /// </summary>
        /// <param name="shoppingService">Shopping service.</param>
        /// <param name="catalogPriceCalculatorFactory">Catalog price calculator factory.</param>
        public RecommendedProductsPriceCalculator(IShoppingService shoppingService, ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory)
        {
            this.shoppingService = shoppingService;
            this.catalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
        }


        /// <summary>
        /// Calculates price for recommended product.
        /// </summary>
        /// <param name="product">Produc.t</param>
        public ProductCatalogPrices CalculatePrice(SKUInfo product)
        {
            var cart = shoppingService.GetCurrentShoppingCart();

            var prices = catalogPriceCalculatorFactory
                         .GetCalculator(cart.ShoppingCartSiteID)
                         .GetPrices(product, Enumerable.Empty<SKUInfo>(), cart);

            return prices;
        }
    }
}