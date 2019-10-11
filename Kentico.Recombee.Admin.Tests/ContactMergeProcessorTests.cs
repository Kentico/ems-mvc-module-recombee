using System;
using System.Collections.Generic;
using System.Linq;

using CMS.ContactManagement;
using CMS.Core;
using CMS.Tests;

using NSubstitute;
using NUnit.Framework;

using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient.Bindings;

namespace Kentico.Recombee.Admin.Tests
{
    [TestFixture]
    public class ContactMergeProcessorTests : UnitTests
    {
        private const string ITEM1 = "11111111-1111-1111-1111-111111111111";
        private const string ITEM2 = "22222222-2222-2222-2222-222222222222";

        [Test]
        public void Process_DataTransferedToTargetContact()
        {
            var recombeeClientService = Substitute.For<IRecombeeClientService>();
            Service.Use<IRecombeeClientService>(recombeeClientService);
            var processor = Service.Resolve<IContactMergeProcessor>();

            var source = Substitute.For<ContactInfo>();
            source.ContactGUID = Guid.NewGuid();

            var target = Substitute.For<ContactInfo>();
            target.ContactGUID = Guid.NewGuid();

            var purchases = new List<Purchase>()
            {
                new Purchase(source.ContactGUID.ToString(), ITEM1),
                new Purchase(source.ContactGUID.ToString(), ITEM2),
            };

            recombeeClientService.GetPurchases(Arg.Is(source)).Returns(purchases);

            processor.Process(source, target);

            Assert.Multiple(() =>
            {
                recombeeClientService.Received().Delete(Arg.Is<IEnumerable<DeletePurchase>>(deleteCommands =>
                        deleteCommands.Any(deleteCommand => deleteCommand.ItemId == ITEM1 && deleteCommand.UserId == source.ContactGUID.ToString())
                        && deleteCommands.Any(deleteCommand => deleteCommand.ItemId == ITEM2 && deleteCommand.UserId == source.ContactGUID.ToString())));

                recombeeClientService.Received().Delete(Arg.Any<IEnumerable<DeleteCartAddition>>());

                recombeeClientService.Received().Add(Arg.Is<IEnumerable<AddPurchase>>(addCommands =>
                    addCommands.Any(addCommand => addCommand.ItemId == ITEM2 && addCommand.UserId == target.ContactGUID.ToString())
                    && addCommands.Any(addCommand => addCommand.ItemId == ITEM2 && addCommand.UserId == target.ContactGUID.ToString())));

                recombeeClientService.Received().Delete(Arg.Any<IEnumerable<DeleteCartAddition>>());

                recombeeClientService.Received().Delete(Arg.Is<DeleteUser>(user => user.UserId == source.ContactGUID.ToString()));
            });
        }
    }
}
