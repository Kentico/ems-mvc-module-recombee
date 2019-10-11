using System.Collections.Generic;

using CMS.ContactManagement;

using Kentico.Recombee.Helpers;

using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient.Bindings;

namespace Kentico.Recombee
{
    /// <summary>
    /// Encapsulates Recombee API.
    /// </summary>
    public class RecombeeClientService : IRecombeeClientService
    {
        private readonly RecombeeClient client;

        /// <summary>
        /// Creates an instance of the <see cref="RecombeeClientService"/> class.
        /// </summary>
        public RecombeeClientService()
        {
            var databaseId = RecommendedProductsSettings.GetDatabaseId();
            var secretToken = RecommendedProductsSettings.GetSecretToken();

            client = new RecombeeClient(databaseId, secretToken);
        }


        /// <summary>
        /// Sends given request to Recombee.
        /// </summary>
        /// <param name="request">Request to be sent.</param>
        public void Send(Request request)
        {
            client.Send(request);
        }


        /// <summary>
        /// Returns collection of <see cref="Purchase"/> made by given <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact.</param>
        public IEnumerable<Purchase> GetPurchases(ContactInfo contact)
        {
            return client.Send(new ListUserPurchases(contact.ContactGUID.ToString()));
        }


        /// <summary>
        /// Returns collection of <see cref="CartAddition"/> made by given <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact.</param>
        public IEnumerable<CartAddition> GetCartAdditions(ContactInfo contact)
        {
            return client.Send(new ListUserCartAdditions(contact.ContactGUID.ToString()));
        }


        /// <summary>
        /// Deletes given purchases from Recombee database.
        /// </summary>
        /// <param name="itemsToDelete">Purchases to delete.</param>
        public void Delete(IEnumerable<DeletePurchase> itemsToDelete)
        {
            Send(itemsToDelete);
        }


        /// <summary>
        /// Deletes given cart additions from Recombee database.
        /// </summary>
        /// <param name="itemsToDelete">Cart additions to delete.</param>
        public void Delete(IEnumerable<DeleteCartAddition> itemsToDelete)
        {
            Send(itemsToDelete);
        }


        /// <summary>
        /// Deletes given user from recombee database.
        /// </summary>
        /// <param name="user">User to delete.</param>
        public void Delete(DeleteUser user)
        {
            Send(user);
        }


        /// <summary>
        /// Adds given purchases to Recombee database.
        /// </summary>
        /// <param name="itemsToAdd">Purchases to add.</param>
        public void Add(IEnumerable<AddPurchase> itemsToAdd)
        {
            Send(itemsToAdd);
        }


        /// <summary>
        /// Adds given cart additions to Recombee database.
        /// </summary>
        /// <param name="itemsToAdd">Cart additions to add.</param>
        public void Add(IEnumerable<AddCartAddition> itemsToAdd)
        {
            Send(itemsToAdd);
        }


        private void Send(IEnumerable<Request> requests)
        {
            client.Send(new Batch(requests));
        }
    }
}
