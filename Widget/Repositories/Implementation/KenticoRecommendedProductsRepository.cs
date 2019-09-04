using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;

namespace Kentico.Recombee.Repositories.Implementation
{
    public class KenticoRecommendedProductsRepository
    {
        /// <summary>
        /// Returns an enumerable collection of products ordered by the date of publication.
        /// </summary>
        /// <param name="filter">Instance of a product filter.</param>
        /// <param name="count">The number of coffees to return. Use 0 as value to return all records.</param>
        /// <returns>An enumerable collection that contains the specified number of coffees.</returns>
        public IEnumerable<SKUTreeNode> GetRecommendedProducts(IList<Guid> recommendations)
        {
            var recommendationWhere = new WhereCondition();
            if (recommendations.Any())
            {
                recommendationWhere.WhereIn("DocumentGuid", recommendations);
            }

            return DocumentHelper.GetDocuments()
                .Published()
                .OnSite(SiteContext.CurrentSiteName)
                .Culture("en-us")
                .CombineWithDefaultCulture()
                .TopN(recommendations.Count)
                .WhereTrue("SKUEnabled")
                .Where(recommendationWhere)
                .OrderByDescending("SKUInStoreFrom")
                .OfType<SKUTreeNode>()
                .ToList();
        }
    }
}