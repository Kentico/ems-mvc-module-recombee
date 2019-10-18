﻿using System.Collections.Generic;

using CMS;
using CMS.ContactManagement;

using Kentico.Recombee;

using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient.Bindings;

[assembly: RegisterImplementation(typeof(IRecombeeClientService), typeof(RecombeeClientService))]

namespace Kentico.Recombee
{
    /// <summary>
    /// Contract for Recombee database.
    /// </summary>
    public interface IRecombeeClientService
    {
        /// <summary>
        /// Sends given request to Recombee.
        /// </summary>
        /// <param name="request">Request to be sent.</param>
        void Send(Request request);

       
        /// <summary>
        /// Returns collection of <see cref="Purchase"/> made by given <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact.</param>
        IEnumerable<Purchase> GetPurchases(ContactInfo contact);


        /// <summary>
        /// Returns collection of <see cref="CartAddition"/> made by given <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact.</param>
        IEnumerable<CartAddition> GetCartAdditions(ContactInfo contact);


        /// <summary>
        /// Deletes given purchases from Recombee database.
        /// </summary>
        /// <param name="itemsToDelete">Purchases to delete.</param>
        void Delete(IEnumerable<DeletePurchase> requests);


        /// <summary>
        /// Deletes given cart additions from Recombee database.
        /// </summary>
        /// <param name="itemsToDelete">Cart additions to delete.</param>
        void Delete(IEnumerable<DeleteCartAddition> deleteCartAdditions);


        /// <summary>
        /// Deletes given user from recombee database.
        /// </summary>
        /// <param name="user">User to delete.</param>
        void Delete(DeleteUser user);


        /// <summary>
        /// Adds given purchases to Recombee database.
        /// </summary>
        /// <param name="itemsToAdd">Purchases to add.</param>
        void Add(IEnumerable<AddPurchase> itemsToAdd);


        /// <summary>
        /// Adds given cart additions to Recombee database.
        /// </summary>
        /// <param name="itemsToAdd">Cart additions to add.</param>
        void Add(IEnumerable<AddCartAddition> itemsToAdd);
    }
}