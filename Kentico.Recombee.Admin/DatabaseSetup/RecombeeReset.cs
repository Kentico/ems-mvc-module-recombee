using System;

using CMS.DataEngine;
using Kentico.Recombee.Helpers;
using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee.DatabaseSetup
{
    public class RecombeeReset
    {
        private readonly RecombeeClient client;

        public RecombeeReset()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


        public void ResetDatabase()
        {
            if (!(CMSApplication.ApplicationInitialized ?? false))
            {
                throw new InvalidOperationException("CMS is not initialized.");
            }

            client.Send(new ResetDatabase());
        }
    }
}
