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
    public class RecombeeClientService
    {
        private RecombeeClient client;

        public RecombeeClientService()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


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