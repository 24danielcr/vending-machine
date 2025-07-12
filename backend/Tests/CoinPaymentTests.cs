using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Application;
using backend.Database;
using backend.Domain;
using backend.Infraestructure;
using Moq;
using Newtonsoft.Json.Linq;

namespace Tests
{
  class CoinPaymentTests
  {
    private Mock<ICoinPaymentRepository> coinPaymentRepository;
    private CoinPaymentCommand coinPaymentCommand;

    [SetUp]
    public void SetUp()
    {
      coinPaymentRepository = new Mock<ICoinPaymentRepository>();
      coinPaymentCommand = new CoinPaymentCommand(coinPaymentRepository.Object);
    }

    [Test]
    public void GetPaymentChange_ShouldReturnTrue_WhenExactChangePossible()
    {

      var vendingCoins = new Dictionary<CoinDenomination
        , VendingMachineMoneyModel>
        {
          {
            CoinDenomination.Colon500,
            new VendingMachineMoneyModel
            {
              Denomination = CoinDenomination.Colon500,
              Value = 500,
              Quantity = 1
            }
          },
          {
            CoinDenomination.Colon100,
            new VendingMachineMoneyModel
            {
              Denomination = CoinDenomination.Colon100,
              Value = 100,
              Quantity = 2
            }
          },
          {
            CoinDenomination.Colon25,
            new VendingMachineMoneyModel
            {
              Denomination = CoinDenomination.Colon25,
              Value = 25,
              Quantity = 4
            }
          }
        };


      coinPaymentRepository.Setup(r => r.GetAvailableVendingMachineCoins())
        .Returns(vendingCoins);

      var paymentChange = new List<CoinPaymentModel>();
      var totalRemainder = 650;

      var result = coinPaymentCommand.GetPaymentChange(paymentChange
        , totalRemainder);

      Assert.True(result);

      Assert.That(paymentChange.Any(c => c.Value == 500 && c.Quantity == 1)
        , Is.True);

      Assert.That(paymentChange.Any(c => c.Value == 100 && c.Quantity == 1)
        , Is.True);

      Assert.That(paymentChange.Any(c => c.Value == 25 && c.Quantity == 2)
        , Is.True);
    }

    [Test]
    public void GetPaymentChange_ShouldReturnFalse_WhenInsufficientCoins()
    {
      var vendingCoins = new Dictionary<CoinDenomination
        , VendingMachineMoneyModel>
            {
                { 
                  CoinDenomination.Colon25,
                  new VendingMachineMoneyModel 
                  {
                    Denomination = CoinDenomination.Colon25,
                    Value = 25,
                    Quantity = 1 
                  }
                }
            };

      var paymentChange = new List<CoinPaymentModel>();
      var totalRemainder = 100;

      var expectedPaymentChangeCount = 0;

      coinPaymentRepository.Setup(r => r.GetAvailableVendingMachineCoins())
        .Returns(vendingCoins);

      var result = coinPaymentCommand.GetPaymentChange(paymentChange
        , totalRemainder);

      Assert.False(result);
    }

    [Test]
    public void ProcessCoins_ShouldThrowException_WhenChangeNotPossible()
    {
      var vendingCoins = new Dictionary<CoinDenomination
        , VendingMachineMoneyModel>
            {
                {
                  CoinDenomination.Colon50,
                  new VendingMachineMoneyModel
                  {
                    Denomination = CoinDenomination.Colon50,
                    Value = 50,
                    Quantity = 0
                  }
                }
            };

      var expectedErrorMsg = "Error in application layer: ProcessCoins";

      coinPaymentRepository.Setup(r => r.GetAvailableVendingMachineCoins())
        .Returns(vendingCoins);

      var coinsPayment = new List<CoinPaymentModel>
      {
        new CoinPaymentModel { Value = 100, Quantity = 1 }
      };

      var ex = Assert.Throws<Exception>(() => coinPaymentCommand
      .ProcessCoins(coinsPayment, 50));

      Assert.That(ex.Message.Contains(expectedErrorMsg));
    }

    [Test]
    public void ProcessCoins_ShouldReturnCorrectChange_AndCallRepo()
    {
      var vendingCoins = new Dictionary<CoinDenomination
              , VendingMachineMoneyModel>
            {
                {
                  CoinDenomination.Colon100,
                  new VendingMachineMoneyModel
                  {
                    Denomination = CoinDenomination.Colon100,
                    Value = 100,
                    Quantity = 2
                  }
                }
            };

      var expectedResultCount = 1;

      coinPaymentRepository.Setup(r => r.GetAvailableVendingMachineCoins())
        .Returns(vendingCoins);

      var coinsPayment = new List<CoinPaymentModel>
      {
        new CoinPaymentModel { Value = 500, Quantity = 1 }
      };

      var result = coinPaymentCommand.ProcessCoins(coinsPayment, 200);

      Assert.That(result.Count, Is.EqualTo(expectedResultCount));

      Assert.True(result.All(c => c.Value == 100));

      Assert.True(result.All(c => c.Quantity == 2));

      coinPaymentRepository.Verify(r => r.ProcessCoins(
        It.IsAny<List<CoinPaymentModel>>()), Times.Once);
    }

    [Test]
    public void ProcessCoins_ShouldReturnEmpty_WhenInputIsNull()
    {
      var result = coinPaymentCommand.ProcessCoins(null, 100);

      Assert.That(result.Count, Is.EqualTo(0));

      coinPaymentRepository.Verify(r => r.ProcessCoins(
        It.IsAny<List<CoinPaymentModel>>()), Times.Never);
    }
  }
}
