using System;
using System.Collections.Generic;
using System.Linq;

using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;

using Kentico.Recombee.Helpers;

using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee.DatabaseSetup
{
    /// <summary>
    /// Encapsulates generating of history data into Recombee.
    /// </summary>
    public class HistoryData
    {
        private const string SITENAME = "DancingGoatMvc";

        private readonly RecombeeClient client;


        /// <summary>
        /// Creates an instace of the <see cref="HistoryData"/> class.
        /// </summary>
        public HistoryData()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


        public void CreateHistoryData()
        {
            if (!(CMSApplication.ApplicationInitialized ?? false))
            {
                throw new InvalidOperationException("CMS is not initialized.");
            }
           
            var coffees = GetProducts("DancingGoatMvc.Coffee");
            var brewers = GetProducts("DancingGoatMvc.Brewer");

            var articles = GetArticles();

            var contacts = GetContacts();

            PushProducts(coffees.Concat(brewers));
            PushArticles(articles);

            PushContacts(contacts);

            PushInteractions(client, contacts, coffees, brewers, articles);
        }


        private static IList<ContactInfo> GetContacts()
        {
            var generator = new ContactsGenerator();
            return generator.Generate();
        }


        private IList<SKUTreeNode> GetProducts(string className)
        {
            return DocumentHelper.GetDocuments(className)
                         .PublishedVersion()
                         .Published()
                         .OnSite(SITENAME)
                         .WhereTrue("SKUEnabled")
                         .OfType<SKUTreeNode>().ToList();
        }


        private IList<TreeNode> GetArticles()
        {
            return DocumentHelper.GetDocuments("DancingGoatMvc.Article")
                          .PublishedVersion()
                          .Published()
                          .OnSite(SITENAME)
                          .ToList();
        }


        private static void PushInteractions(RecombeeClient client, IList<ContactInfo> contacts, IList<SKUTreeNode> allCoffees, IList<SKUTreeNode> allBrewers, IList<TreeNode> allArticles)
        {
            var coffeeArticles = GetArticlesAboutCoffees(allArticles);
            var purchasedCoffees = GetPurchasedCaffees(allCoffees);
            var cofferBuyers = contacts.Take(contacts.Count / 2);
            var brewerBuyers = contacts.Skip(contacts.Count / 2).Take(contacts.Count / 2);

            var interactions = new List<Request>();

            foreach (var coffeeBuyer in cofferBuyers)
            {

                foreach (var article in coffeeArticles)
                {
                    interactions.Add(new AddDetailView(coffeeBuyer.ContactGUID.ToString(), article.DocumentGUID.ToString(), cascadeCreate: true));
                }

                foreach (var coffee in purchasedCoffees)
                {
                    interactions.Add(new AddCartAddition(coffeeBuyer.ContactGUID.ToString(), coffee.DocumentGUID.ToString(), cascadeCreate: true));
                    interactions.Add(new AddPurchase(coffeeBuyer.ContactGUID.ToString(), coffee.DocumentGUID.ToString(), cascadeCreate: true));
                }
            }

            var brewerArticle = GetArticleAboutBrewers(allArticles);
            var purchasedBrewers = GetPurchasedBrewers(allBrewers);

            foreach (var brewerBuyer in brewerBuyers)
            {
                interactions.Add(new AddDetailView(brewerBuyer.ContactGUID.ToString(), brewerArticle.DocumentGUID.ToString(), cascadeCreate: true));

                foreach (var brewer in purchasedBrewers)
                {
                    interactions.Add(new AddCartAddition(brewerBuyer.ContactGUID.ToString(), brewer.DocumentGUID.ToString(), cascadeCreate: true));
                    interactions.Add(new AddPurchase(brewerBuyer.ContactGUID.ToString(), brewer.DocumentGUID.ToString(), cascadeCreate: true));
                }
            }

            client.Send(new Batch(interactions));
        }


        private static IEnumerable<SKUTreeNode> GetPurchasedCaffees(IList<SKUTreeNode> allCoffees)
        {
            return allCoffees.Where(coffee =>
                coffee.NodeAlias.Equals("Brazil-Natural-Barra-Grande", StringComparison.InvariantCultureIgnoreCase) ||
                coffee.NodeAlias.Equals("Bolivia-Finca-Illimani", StringComparison.InvariantCultureIgnoreCase) ||
                coffee.NodeAlias.Equals("Colombia-Carlos-Imbachi", StringComparison.InvariantCultureIgnoreCase));
        }


        private static IEnumerable<SKUTreeNode> GetPurchasedBrewers(IList<SKUTreeNode> allBrewers)
        {
            return allBrewers.Where(coffee =>
                coffee.NodeAlias.Equals("Chemex", StringComparison.InvariantCultureIgnoreCase) ||
                coffee.NodeAlias.Equals("Clever-Dripper", StringComparison.InvariantCultureIgnoreCase) ||
                coffee.NodeAlias.Equals("Hario-V60", StringComparison.InvariantCultureIgnoreCase));
        }


        private static IEnumerable<TreeNode> GetArticlesAboutCoffees(IList<TreeNode> allArticles)
        {
            return allArticles.Where(article =>
                article.NodeAlias.Equals("Coffee-Beverages-Explained", StringComparison.InvariantCultureIgnoreCase) ||
                article.NodeAlias.Equals("Coffee-processing-techniques", StringComparison.InvariantCultureIgnoreCase) ||
                article.NodeAlias.Equals("On-Roasts", StringComparison.InvariantCultureIgnoreCase));
        }


        private static TreeNode GetArticleAboutBrewers(IList<TreeNode> allArticles)
        {
            return allArticles.Where(article =>
                article.NodeAlias.Equals("Which-brewing-fits-you", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }


        private void PushContacts(IEnumerable<ContactInfo> contacts)
        {
            var customersToPush = contacts.Select(x => new SetUserValues(x.ContactGUID.ToString(), new Dictionary<string, object>
            {
                { "FirstName", x.ContactFirstName },
                { "LastName", x.ContactLastName },
                { "Email", x.ContactEmail },
            }, true));

            client.Send(new Batch(customersToPush));
        }


        private void PushArticles(IList<TreeNode> articles)
        {
            var articlesToPush = articles.Select(productPage => new SetItemValues(productPage.DocumentGUID.ToString(),
                new Dictionary<string, object>
                {
                    { "Name", productPage.DocumentName },
                    { "Description", productPage.GetValue("ArticleSummary") },
                    { "Content", productPage.GetValue("ArticleText")},
                    { "Culture", productPage.DocumentCulture},
                    { "ClassName", productPage.ClassName},
                    { "Type", "Article"},

                }, true));

            client.Send(new Batch(articlesToPush));
        }


        private void PushProducts(IEnumerable<SKUTreeNode> products)
        {
            var productsToPush = products.Select(productPage => new SetItemValues(productPage.DocumentGUID.ToString(),
                new Dictionary<string, object>
                {
                    { "Name", productPage.DocumentSKUName },
                    { "Description", productPage.DocumentSKUShortDescription },
                    { "Content", productPage.DocumentSKUDescription},
                    { "Culture", productPage.DocumentCulture},
                    { "ClassName", productPage.ClassName},
                    { "Price", productPage.SKU.SKUPrice },
                    { "Type", "Product"},

                }, true));

            client.Send(new Batch(productsToPush));
        }
    }
}
