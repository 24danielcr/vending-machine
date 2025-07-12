using backend.Application;
using backend.Infraestructure;
using backend.Domain;

using Moq;

namespace Tests
{
  public class ProductPaymentTests
  {

    private ProductPaymentCommand productPaymentCommand;
    private Mock<IProductPaymentRepository> productPaymentRepository;

    [SetUp]
    public void Setup()
    {
      productPaymentRepository = new Mock<IProductPaymentRepository>();
      productPaymentCommand = new ProductPaymentCommand(
        productPaymentRepository.Object);
    }

    [Test]
    public void BuyProducts_ShouldFormatProductName_WhenSpaces()
    {
      var productName = "Coca Cola";
      var productQuantity = 1;

      var productVMName = "CocaCola";
      var productVMQuantity = 10;

      var products = new List<ProductPaymentModel>
            {
                new ProductPaymentModel
                {
                  Name = productName,
                  Quantity = productQuantity
                }
            };

      productPaymentRepository.Setup(r => r.GetProductQuantity(productVMName))
        .Returns(productVMQuantity);

      var result = productPaymentCommand.BuyProducts(products);

      Assert.That(string.Empty, Is.EqualTo(result));
      Assert.That(productVMName, Is.EqualTo(products[0].Name));
    }

    [Test]
    public void BuyProducts_ShouldReturnError_WhenQuantityInsufficient()
    {
      var productName = "Pepsi";
      var productQuantity = 5;

      var productVMName = "Pepsi";
      var productVMQuantity = 2;

      var expectedErrorMsg = "Insufficient " + productVMName + " quantity.";

      var products = new List<ProductPaymentModel>
            {
                new ProductPaymentModel
                {
                  Name = productName,
                  Quantity = productQuantity
                }
            };

      productPaymentRepository.Setup(r => r.GetProductQuantity(productVMName))
        .Returns(productVMQuantity);

      var result = productPaymentCommand.BuyProducts(products);

      Assert.That(result.Contains(expectedErrorMsg));
    }

    [Test]
    public void BuyProducts_ShouldReturnError_WhenProductsIsNull()
    {
      var expectedErrorMsg = "No products were selected. ";

      var result = productPaymentCommand.BuyProducts(null);

      Assert.That(expectedErrorMsg, Is.EqualTo(result));
    }

    [Test]
    public void ConfirmBuyProducts_Should_CallRepositoryMethod()
    {
      var productName = "Fanta";
      var productQuantity = 1;

      var products = new List<ProductPaymentModel>
            {
                new ProductPaymentModel
                {
                  Name = productName,
                  Quantity = productQuantity
                }
            };

      productPaymentCommand.ConfirmBuyProducts(products);

      productPaymentRepository.Verify(r => r.BuyProducts(products), Times.Once);
    }

    [Test]
    public void ConfirmBuyProducts_ShouldThrowException_WhenError()
    {
      var productName = "Sprite";
      var productQuantity = 1;

      var exceptionMsg = "Error in application layer: ConfirmBuyProducts.";

      var products = new List<ProductPaymentModel>
            {
                new ProductPaymentModel
                {
                  Name = productName,
                  Quantity = productQuantity
                }
            };

      productPaymentRepository
          .Setup(r => r.BuyProducts(products))
          .Throws(new ArgumentException(exceptionMsg));

      var ex = Assert.Throws<ArgumentException>(() =>
          productPaymentCommand.ConfirmBuyProducts(products));

      Assert.That(exceptionMsg.Contains(ex.Message));
    }
  }
}