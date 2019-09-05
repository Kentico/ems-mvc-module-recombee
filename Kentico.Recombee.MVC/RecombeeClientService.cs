using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Core;

using Kentico.Recombee.Helpers;

using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient.Bindings;

namespace Kentico.Recombee
{
    /// <summary>
    /// Encapsulates Recombee API.
    /// </summary>
    public class RecombeeClientService
    {
        private RecombeeClient client;

        /// <summary>
        /// Creates an instace of the <see cref="RecombeeClientService"/> class.
        /// </summary>
        public RecombeeClientService()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


        /// <summary>
        /// Recommends a product to given <paramref name="contactGuid"/>.
        /// </summary>
        /// <param name="contactGuid">Contact for which personalized recommendations are to be generated.</param>
        /// <param name="count">Number of products to be recommended.</param>
        /// <returns></returns>
        public IList<Guid> RecommendProductsToContact(Guid contactGuid, int count)
        {
            var result = default(IList<Guid>);
            try
            {
                var recommendation = client.Send(
                    new RecommendItemsToUser(contactGuid.ToString(), count, cascadeCreate: true, filter: $"('Type' == \"Product\")", logic: new Logic("recombee:personal"))
                );

                result = recommendation.Recomms.Select(x => Guid.Parse(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                result = new List<Guid>();
                Service.Resolve<IEventLogService>().LogException("Recombee integration module", "RECOMMENDATION", ex);

            }

            return result;
        }


        /// <summary>
        /// Logs page view
        /// </summary>
        /// <param name="contactGuid">Contact who performed the page view.</param>
        /// <param name="pageGuid">Visited page.</param>
        public void LogPageView(Guid contactGuid, Guid pageGuid)
        {
            try
            {
                client.Send(new AddDetailView(contactGuid.ToString(), pageGuid.ToString(), cascadeCreate: true));
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("Recombee integration module", "LOGPAGEVIEW", ex);
            }
        }

        /// <summary>
        /// Logs product addition.
        /// </summary>
        /// <param name="contactGuid">Contact who added the item to the cart.</param>
        /// <param name="productGuid">Product added to the cart.</param>
        /// <param name="amount">Amount of added items.</param>
        public void LogProductAddition(Guid contactGuid, Guid productGuid, int amount)
        {
            try
            {
                client.Send(new AddCartAddition(contactGuid.ToString(), productGuid.ToString(), cascadeCreate: true, amount: amount));
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("Recombee integration module", "LOGPRODUCTADDITION", ex);
            }
        }



        /// <summary>
        /// Logs product purchased.
        /// </summary>
        /// <param name="contactGuid">Contact who performed the purchased.</param>
        /// <param name="productGuid">Purchased product</param>
        /// <param name="amount">Amount.</param>
        public void LogProductPurchase(Guid contactGuid, Guid productGuid, int amount)
        {
            try
            {
                client.Send(new AddPurchase(contactGuid.ToString(), productGuid.ToString(), cascadeCreate: true, amount: amount));
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("Recombee integration module", "LOGPRODUCTPURCHASE", ex); ;
            }
        }
    }
}