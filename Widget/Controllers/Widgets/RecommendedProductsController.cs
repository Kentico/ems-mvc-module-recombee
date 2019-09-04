using System.Linq;
using System.Web.Mvc;

using CMS.ContactManagement;
using CMS.Ecommerce;
using CMS.Core;

using Kentico.PageBuilder.Web.Mvc;

using Kentico.Recombee.Controllers.Widgets;
using Kentico.Recombee.Helpers;
using Kentico.Recombee.Repositories.Implementation;
using Kentico.Recombee.Models.Widgets.RecommendedProducts;

[assembly: RegisterWidget("Kentico.Recombee.Widget.RecommendedProducts", typeof(RecommendedProductsController), "{$Kentico.Recombee.Widget.RecommendedProducts.name$}", Description = "{$Kentico.Recombee.Widget.RecommendedProducts.description$}", IconClass = "icon-star-full")]

namespace Kentico.Recombee.Controllers.Widgets
{
    /// <summary>
    /// Controller for recommended products widgets.
    /// </summary>
    public class RecommendedProductsController : WidgetController
    {
        private const int PRODUCTS_COUNT = 4;

        private readonly RecommendedProductsPriceCalculator priceCalculator;
        private readonly KenticoRecommendedProductsRepository repository;
        private readonly RecombeeClientService recombee;


        /// <summary>
        /// Creates an instance of <see cref="ArticlesWidgetController"/> class.
        /// </summary>
        /// <param name="repository">Repository for retrieving articles.</param>
        /// <param name="outputCacheDependencies">Output cache dependency handling.</param>
        public RecommendedProductsController()
        {
            priceCalculator = new RecommendedProductsPriceCalculator(Service.Resolve<IShoppingService>(), Service.Resolve<ICatalogPriceCalculatorFactory>());

            repository = new KenticoRecommendedProductsRepository();
            recombee = new RecombeeClientService();
        }


        public ActionResult Index()
        {
            var contact = ContactManagementContext.GetCurrentContact();

            if (contact == null)
            {
                return new ContentResult() { Content = "Enable online marketing." };
            }

            var products = repository.GetRecommendedProducts(recombee.RecommendProductsToContact(contact.ContactGUID, PRODUCTS_COUNT));

            var items = products.Select(product => new RecommendedProductViewModel(product, priceCalculator.CalculatePrice(product.SKU)));
            var model = new RecommendedProductsViewModel { Products = items };

            return PartialView("Widgets/_RecommendedProducts", model);
        }
    }
}