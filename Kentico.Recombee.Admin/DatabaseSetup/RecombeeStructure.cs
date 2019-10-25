using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;

using Kentico.Recombee.Helpers;
using Kentico.Recombee.Mappers;
using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee.DatabaseSetup
{
    /// <summary>
    /// Initialize Recombee database structure.
    /// </summary>
    public class RecombeeStructure
    {
        private readonly RecombeeClient client;

        /// <summary>
        /// Creates an instance of the <see cref="RecombeeStructure"/> class.
        /// </summary>
        /// <param name="databaseId">Recombee database identifier.</param>
        /// <param name="secretToken">Recombee database secret token.</param>
        public RecombeeStructure()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


        /// <summary>
        /// Set up Recombee database structure to be able to work with EMS items.
        /// </summary>
        public void SetupDatabaseStructure()
        {
            if (!(CMSApplication.ApplicationInitialized ?? false))
            {
                throw new InvalidOperationException("CMS is not initialized.");
            }

            InitializeDBStructureForProducts();
            InitializeDBStructureForContacts();


            PushProducts(GetProducts());
        }


        private void InitializeDBStructureForProducts()
        {
            client.Send(new AddItemProperty("Name", "string"));
            client.Send(new AddItemProperty("Description", "string"));
            client.Send(new AddItemProperty("Price", "double"));
            client.Send(new AddItemProperty("Type", "string"));
            client.Send(new AddItemProperty("ClassName", "string"));
            client.Send(new AddItemProperty("Content", "string"));
            client.Send(new AddItemProperty("Culture", "string"));
        }


        private void InitializeDBStructureForContacts()
        {
            client.Send(new AddUserProperty("FirstName", "string"));
            client.Send(new AddUserProperty("LastName", "string"));
            client.Send(new AddUserProperty("Email", "string"));
        }


        private IList<SKUTreeNode> GetProducts()
        {
            var siteService = Service.Resolve<ISiteService>();
            return DocumentHelper.GetDocuments()
                         .PublishedVersion()
                         .Published()
                         .OnSite(siteService.CurrentSite?.SiteName)
                         .WhereTrue("SKUEnabled")
                         .OfType<SKUTreeNode>().ToList();
        }

        private void PushProducts(IEnumerable<SKUTreeNode> products)
        {
            var productsToPush = products.Select(productPage => new SetItemValues(
                productPage.DocumentGUID.ToString(),
                ProductMapper.Map(productPage),
                true)
            );

            client.Send(new Batch(productsToPush));
        }
    }
}
