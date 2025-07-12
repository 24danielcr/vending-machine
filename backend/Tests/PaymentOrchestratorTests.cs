using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Application;
using backend.Database;
using backend.Domain;
using backend.Infraestructure;
using Moq;

namespace Tests
{
  class PaymentOrchestratorTests
  {
    private Mock<ICoinPaymentCommand> coinPaymentCommand;
    private Mock<IProductPaymentCommand> productPaymentCommand;
    private Mock<IPaymentOrchestratorRepository> paymentOrchestratorRepository;

    private PaymentOrchestrator paymentOrchestrator;

    [SetUp]
    public void SetUp()
    {
      coinPaymentCommand = new Mock<ICoinPaymentCommand>();
      productPaymentCommand = new Mock<IProductPaymentCommand>();
      paymentOrchestratorRepository = new Mock<IPaymentOrchestratorRepository>();

      paymentOrchestrator = new PaymentOrchestrator(coinPaymentCommand.Object
        , productPaymentCommand.Object, paymentOrchestratorRepository.Object);
    }

    private List<CoinPaymentModel> GetTestCoins() => new()
    {
        new CoinPaymentModel { Value = 500, Quantity = 4 },
        new CoinPaymentModel { Value = 50, Quantity = 1 }
    };

    private List<ProductPaymentModel> GetTestProducts() => new()
    {
        new ProductPaymentModel { Name = "CocaCola", Quantity = 1 },
        new ProductPaymentModel { Name = "Sprite", Quantity = 1 }
    };

    [Test]
    public void ProcessPayment_ShouldReturnChange_WhenPaymentSuccessful()
    {
      var coins = GetTestCoins();
      var products = GetTestProducts();

      productPaymentCommand.Setup(p => p.BuyProducts(products)).Returns(
        string.Empty);

      paymentOrchestratorRepository.Setup(r => r.GetProductPrice("CocaCola"))
        .Returns(800);

      paymentOrchestratorRepository.Setup(r => r.GetProductPrice("Sprite"))
        .Returns(975);

      var expectedChange = new List<CoinPaymentModel>
        {
            new CoinPaymentModel { Value = 100, Quantity = 2 },
            new CoinPaymentModel { Value = 50, Quantity = 1 },
            new CoinPaymentModel { Value = 25, Quantity = 1 },
        };

      coinPaymentCommand.Setup(c => c.ProcessCoins(coins, 275))
          .Returns(expectedChange);

      var result = paymentOrchestrator.ProcessPayment(coins, products);

      productPaymentCommand.Verify(p => p.ConfirmBuyProducts(products)
      , Times.Once);

      Assert.That(expectedChange, Is.EqualTo(result));
    }

    [Test]
    public void ProcessPayment_ShouldThrow_WhenBuyProductError()
    {
      var coins = GetTestCoins();
      var products = GetTestProducts();

      var expectedErrorMsg = "Error in application layer: ProcessPayment.";

      productPaymentCommand.Setup(p => p.BuyProducts(products))
            .Returns("Insufficient CocaCola quantity.");

      var ex = Assert.Throws<Exception>(() =>
          paymentOrchestrator.ProcessPayment(coins, products));

      productPaymentCommand.Verify(p => p.ConfirmBuyProducts(
        It.IsAny<List<ProductPaymentModel>>()), Times.Never);
      
      Assert.True(ex.Message.Contains(expectedErrorMsg));
    }

    [Test]
    public void ProcessPayment_ShouldThrow_WhenUserPaysLessThanProductCost()
    {
      var coins = GetTestCoins();
      var products = GetTestProducts();

      var expectedErrorMsg = "Error in application layer: ProcessPayment.";

      productPaymentCommand.Setup(p => p.BuyProducts(products))
        .Returns(string.Empty);

      paymentOrchestratorRepository
        .Setup(r => r.GetProductPrice("CocaCola")).Returns(800);

      paymentOrchestratorRepository
        .Setup(r => r.GetProductPrice("Sprite")).Returns(2000);

      var ex = Assert.Throws<Exception>(() =>
          paymentOrchestrator.ProcessPayment(coins, products));

      coinPaymentCommand.Verify(c => c.ProcessCoins(
        It.IsAny<List<CoinPaymentModel>>(), It.IsAny<int>()), Times.Never);

      productPaymentCommand.Verify(p => p.ConfirmBuyProducts(
        It.IsAny<List<ProductPaymentModel>>()), Times.Never);
     
      Assert.True(ex.Message.Contains(expectedErrorMsg));
    }
  }
}
