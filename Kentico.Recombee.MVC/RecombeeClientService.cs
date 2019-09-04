using System;
using System.Collections.Generic;
using System.Linq;
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
            RecommendationResponse result = client.Send(
                new RecommendItemsToUser(contactGuid.ToString(), count, cascadeCreate: true, filter: $"('Type' == \"Product\")", logic: new Logic("recombee:personal"))
            );
            return result.Recomms.Select(x => Guid.Parse(x.Id)).ToList();
        }


        public void LogPageView(Guid contactGuid, Guid pageGuid)
        {
            client.Send(new AddDetailView(contactGuid.ToString(), pageGuid.ToString(), cascadeCreate: true));
        }


        public void LogProductAddition(Guid contactGuid, Guid productGuid, int amount)
        {
            client.Send(new AddCartAddition(contactGuid.ToString(), productGuid.ToString(), cascadeCreate: true, amount: amount));
        }


        public void LogProductPurchase(Guid contactGuid, Guid productGuid, int amount)
        {
            client.Send(new AddPurchase(contactGuid.ToString(), productGuid.ToString(), cascadeCreate: true, amount: amount));
        }
    }
}