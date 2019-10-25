using CMS.Core;
using CMS.Ecommerce;
using CMS.Tests;

using NSubstitute;
using NUnit.Framework;

namespace Kentico.Recombee.Admin.Tests
{
    [TestFixture]
    public class ProductUpdatesProcessorTests : UnitTests
    {
        [Test]
        public void Test()
        {
            var clientService = Substitute.For<IRecombeeClientService>();
            Service.Use<IRecombeeClientService>(clientService);
            var productPage = Substitute.For<SKUTreeNode>();

            var processor = Service.Resolve<IProductUpdatesProcessor>();
            processor.ProcessNewProduct(productPage);

            clientService.Received().UpdateProduct(Arg.Is(productPage));
        }
    }
}
