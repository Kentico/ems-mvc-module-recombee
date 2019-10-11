using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;

using Kentico.Recombee;
using Kentico.Recombee.Helpers;
using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;

[assembly: RegisterModule(typeof(RecombeeModule))]

namespace Kentico.Recombee
{
    /// <summary>
    /// Recombee module for MVC instance.
    /// </summary>
    public class RecombeeModule : Module
    {
        /// <summary>
        /// Creates an instance of the <see cref="RecombeeModule"/> class.
        /// </summary>
        public RecombeeModule() : base("RecombeeAdminModule", false)
        {
           
        }


        protected override void OnInit()
        {
            base.OnInit();
            DocumentEvents.Insert.After += ProductCreated;
            ContactManagementEvents.ContactMerge.Execute += ContactMerge_Execute;
        }

        private void ProductCreated(object sender, DocumentEventArgs e)
        {
            var document = e.Node;

            if (!document.IsProduct())
            {
                return;
            }

            if (!(document is SKUTreeNode productPage))
            {
                return;
            }

            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();
            var client = new RecombeeClient(databaseId, secretToken);

            var productsToPush = new SetItemValues(productPage.DocumentGUID.ToString(),
                new Dictionary<string, object>
                {
                    { "Name", productPage.DocumentSKUName },
                    { "Description", productPage.DocumentSKUShortDescription },
                    { "Content", productPage.DocumentSKUDescription},
                    { "Culture", productPage.DocumentCulture},
                    { "ClassName", productPage.ClassName},
                    { "Price", productPage.SKU.SKUPrice },
                    { "Type", "Product"},
                }, true);

            try
            {
                client.Send(productsToPush);
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("RecombeeAdminModule", "ON_PRODUCT_CREATED", ex);
            }
        }


        private void ContactMerge_Execute(object sender, CMSEventArgs<ContactMergeModel> e)
        {
            var service = Service.Resolve<IContactMergeProcessor>();
            service.Process(e.Parameter.SourceContact, e.Parameter.TargetContact);
        }
    }
}
