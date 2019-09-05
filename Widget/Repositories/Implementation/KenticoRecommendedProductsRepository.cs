using System;
using System.Collections.Generic;
using System.Linq;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;

namespace Kentico.Recombee.Repositories.Implementation
{
    /// <summary>
    /// Repository for featured products.
    /// </summary>
    public class KenticoRecommendedProductsRepository
    {
        /// <summary>
        /// Returns an enumerable collection of products ordered by the date of publication.
        /// If no recommendation provided, top <paramref name="count"/> products is returned.
        /// </summary>
        /// <param name="filter">Instance of a product filter.</param>
        /// <param name="count">The number of products to return. Use 0 as value to return all records.</param>
        /// <returns>An enumerable collection that contains the specified number of products.</returns>
        public IEnumerable<SKUTreeNode> GetRecommendedProducts(IList<Guid> recommendations, int count = 4)
        {
            AddMissingRecommendations(recommendations, count);

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
                .TopN(count)
                .WhereTrue("SKUEnabled")
                .Where(recommendationWhere)
                .OrderByDescending("SKUInStoreFrom")
                .OfType<SKUTreeNode>()
                .ToList();
        }

        private static void AddMissingRecommendations(IList<Guid> recommendations, int count)
        {
            if (recommendations.Count < count)
            {
                var productGuids = DocumentHelper.GetDocuments()
                .Published()
                .OnSite(SiteContext.CurrentSiteName)
                .Culture("en-us")
                .CombineWithDefaultCulture()
                .TopN(count - recommendations.Count)
                .WhereTrue("SKUEnabled")
                .WhereNotIn("DocumentGuid", recommendations)
                .OrderByDescending("SKUInStoreFrom")
                .Columns("DocumentGuid")
                .GetListResult<Guid>()
                .ToList();

                productGuids.ForEach(guid => recommendations.Add(guid));
            }
        }
    }
}
