using CMS.Ecommerce;

using Kentico.Recombee.Mappers;

using NUnit.Framework;
using NSubstitute;

namespace Kentico.Recombee.Admin.Tests
{
    [TestFixture]
    public class ProductMapperTests
    {
        [Test]
        public void Map_ProductIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => { ProductMapper.Map(null); }, Throws.ArgumentNullException);
        }


        [Test]
        public void Map_ProductPropertiesMappedToCorrectStructure()
        {
            var sku = Substitute.For<SKUInfo>();
            sku.SKUPrice.Returns(99.9M);
            sku.SKUID.Returns(1);

            var productPage = Substitute.For<SKUTreeNode>();
            productPage.SKU = sku;
            productPage.DocumentSKUName.Returns("DocumentSKUName");
            productPage.DocumentSKUShortDescription.Returns("DocumentSKUShortDescription");
            productPage.DocumentSKUDescription.Returns("DocumentSKUDescription");
            productPage.DocumentCulture.Returns("DocumentCulture");
            productPage.ClassName.Returns("ClassName");
            
            var result = ProductMapper.Map(productPage);

            Assert.Multiple(() => {
                Assert.That(result["Name"], Is.EqualTo("DocumentSKUName"));
                Assert.That(result["Description"], Is.EqualTo("DocumentSKUShortDescription"));
                Assert.That(result["Content"], Is.EqualTo("DocumentSKUDescription"));
                Assert.That(result["Culture"], Is.EqualTo("DocumentCulture"));
                Assert.That(result["ClassName"], Is.EqualTo("ClassName"));
                Assert.That(result["Price"], Is.EqualTo(99.9M));
                Assert.That(result["Type"], Is.EqualTo("Product"));
            });
        }
    }
}
