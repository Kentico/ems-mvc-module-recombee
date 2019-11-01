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
        private IProductUpdatesProcessor processor;
        private IRecombeeClientService clientService;

       
        [SetUp]
        public void SetUp()
        {
            clientService = Substitute.For<IRecombeeClientService>();
            Service.Use<IRecombeeClientService>(clientService);

            processor = Service.Resolve<IProductUpdatesProcessor>();
        }


        [Test]
        public void Ctor_NullArgument_ThrowsArgumentNullException()
        {
            Assert.That(() => { new ProductUpdatesProcessor(null); }, Throws.ArgumentNullException);
        }


        [Test]
        public void Methods_NullArgumentt_ThrowsArgumentNullException()
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => { processor.AddProduct(null); }, Throws.ArgumentNullException);
                Assert.That(() => { processor.DeleteProduct(null); }, Throws.ArgumentNullException);
            });
        }


        [Test]
        public void AddProduct_ProductIsAdded()
        {
            var productPage = Substitute.For<SKUTreeNode>();

            processor.AddProduct(productPage);

            clientService.Received().UpdateProduct(Arg.Is(productPage));
        }


        [Test]
        public void DeleteProduct_ProductIsDeleted()
        {
            var productPage = Substitute.For<SKUTreeNode>();

            processor.DeleteProduct(productPage);

            clientService.Received().Delete(Arg.Is(productPage));
        }


        [Test]
        public void UpdateProduct_ProductIsUdated()
        {
            var productPage = Substitute.For<SKUTreeNode>();

            processor.UpdateProduct(productPage);

            clientService.Received().UpdateProduct(Arg.Is(productPage));
        }
    }
}
