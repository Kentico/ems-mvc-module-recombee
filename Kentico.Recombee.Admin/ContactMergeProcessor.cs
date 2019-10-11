using System;
using System.Collections.Generic;

using CMS.ContactManagement;

using Recombee.ApiClient.ApiRequests;

namespace Kentico.Recombee
{
    public class ContactMergeProcessor : IContactMergeProcessor
    {
        private readonly IRecombeeClientService recombeeClientService;

        public ContactMergeProcessor(IRecombeeClientService recombeeClientService)
        {
            this.recombeeClientService = recombeeClientService;
        }


        /// <summary>
        /// Moves all existing purchases and cart additions from <paramref name="source"/> 
        /// contact to <paramref name="target"/> contact.
        /// </summary>
        /// <param name="source">Source contact.</param>
        /// <param name="target">Target contact</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="source"/> or <paramref name="target"/> is null.</exception>
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
            var purchases = recombeeClientService.GetPurchases(source);
            var cardAdditions = recombeeClientService.GetCartAdditions(source);

            var addPurchases = new List<AddPurchase>();
            var addCartAdditions = new List<AddCartAddition>();
            var deletePurchases = new List<DeletePurchase>();
            var deleteCartAdditions = new List<DeleteCartAddition>();

            foreach (var purchase in purchases)
            {
                addPurchases.Add(new AddPurchase(target.ContactGUID.ToString(), purchase.ItemId, amount: purchase.Amount, cascadeCreate: true));
                deletePurchases.Add(new DeletePurchase(purchase.UserId, purchase.ItemId));
            }

            foreach (var cartAddition in cardAdditions)
            {
                addCartAdditions.Add(new AddCartAddition(target.ContactGUID.ToString(), cartAddition.ItemId, amount: cartAddition.Amount, cascadeCreate: true));
                deleteCartAdditions.Add(new DeleteCartAddition(cartAddition.UserId, cartAddition.ItemId));
            }

            recombeeClientService.Delete(deletePurchases);
            recombeeClientService.Delete(deleteCartAdditions);
            recombeeClientService.Add(addPurchases);
            recombeeClientService.Add(addCartAdditions);

            recombeeClientService.Delete(new DeleteUser(source.ContactGUID.ToString()));
        }
    }
}
