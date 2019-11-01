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

            var service = Service.Resolve<IProductUpdatesProcessor>();
            service.AddProduct(productPage);
        }


        private void ContactMerge_Execute(object sender, CMSEventArgs<ContactMergeModel> e)
        {
            var service = Service.Resolve<IContactMergeProcessor>();
            service.Process(e.Parameter.SourceContact, e.Parameter.TargetContact);
        }
    }
}
