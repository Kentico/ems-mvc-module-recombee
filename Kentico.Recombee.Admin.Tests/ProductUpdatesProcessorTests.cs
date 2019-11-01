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
        public void Ctor_NullArgument_ThrowsArgumentNullException()
        {
            Assert.That(() => { new ProductUpdatesProcessor(null); }, Throws.ArgumentNullException);
        }


        [Test]
        public void AddProduct_ProductIsAdded()
        {
            var clientService = Substitute.For<IRecombeeClientService>();
            Service.Use<IRecombeeClientService>(clientService);
            var productPage = Substitute.For<SKUTreeNode>();

            var processor = Service.Resolve<IProductUpdatesProcessor>();
            processor.AddProduct(productPage);

            clientService.Received().UpdateProduct(Arg.Is(productPage));
        }
    }
}
