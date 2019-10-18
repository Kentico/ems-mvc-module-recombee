using System;
using System.Collections.Generic;

using CMS.ContactManagement;

using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee
{
    /// <summary>
    /// Encapsulates transferring user actions from the source user to the target user.
    /// </summary>
    public class ContactMergeProcessor : IContactMergeProcessor
    {
        private readonly IRecombeeClientService recombeeClientService;

        public ContactMergeProcessor(IRecombeeClientService recombeeClientService)
        {
            this.recombeeClientService = recombeeClientService;
        }


        /// <summary>
        /// Moves all existing interactions (purchases and cart additions) of the <paramref name="source"/> 
        /// contact to the <paramref name="target"/> contact.
        /// </summary>
        /// <param name="source">Source contact.</param>
        /// <param name="target">Target contact.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="source"/> or <paramref name="target"/> is null.</exception>
        /// <remarks>When interactions are successfully transferred to <paramref name="target"/>, the <paramref name="source"/> is deleted from Recombee.</remarks>
        public void Process(ContactInfo source, ContactInfo target)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            ProcessInternal(source, target);
        }


        private void ProcessInternal(ContactInfo source, ContactInfo target)
        {
            MovePurchases(source, target);
            MoveCartAdditions(source, target);
            RemoveSourceUser(source);
        }


        private void MovePurchases(ContactInfo source, ContactInfo target)
        {
            var purchases = recombeeClientService.GetPurchases(source);
            var purchasesToAdd = new List<AddPurchase>();
            var purchasesToDelete = new List<DeletePurchase>();

            foreach (var purchase in purchases)
            {
                purchasesToAdd.Add(new AddPurchase(target.ContactGUID.ToString(), purchase.ItemId, amount: purchase.Amount, cascadeCreate: true));
                purchasesToDelete.Add(new DeletePurchase(purchase.UserId, purchase.ItemId));
            }

            recombeeClientService.Delete(purchasesToDelete);
            recombeeClientService.Add(purchasesToAdd);
        }


        private void MoveCartAdditions(ContactInfo source, ContactInfo target)
        {
            var cardAdditions = recombeeClientService.GetCartAdditions(source);
            var cartAdditionsToAdd = new List<AddCartAddition>();
            var cartAdditionsToDelete = new List<DeleteCartAddition>();

            foreach (var cartAddition in cardAdditions)
            {
                cartAdditionsToAdd.Add(new AddCartAddition(target.ContactGUID.ToString(), cartAddition.ItemId, amount: cartAddition.Amount, cascadeCreate: true));
                cartAdditionsToDelete.Add(new DeleteCartAddition(cartAddition.UserId, cartAddition.ItemId));
            }

            recombeeClientService.Delete(cartAdditionsToDelete);
            recombeeClientService.Add(cartAdditionsToAdd);
        }


        private void RemoveSourceUser(ContactInfo source)
        {
            recombeeClientService.Delete(new DeleteUser(source.ContactGUID.ToString()));
        }
    }
}
