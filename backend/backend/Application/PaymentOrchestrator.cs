using System.Runtime.InteropServices;
using backend.Database;
using backend.Domain;
using backend.Infraestructure;

namespace backend.Application
{
  public class PaymentOrchestrator : IPaymentOrchestrator
  {
    private readonly ICoinPaymentCommand coinPaymentCommand;
    private readonly IProductPaymentCommand productPaymentCommand;

    private readonly IPaymentOrchestratorRepository
      paymentOrchestratorRepository;

    public PaymentOrchestrator
      ( ICoinPaymentCommand coinPaymentCommand
      , IProductPaymentCommand productPaymentCommand
      , IPaymentOrchestratorRepository paymentOrchestratorRepository
      )
    {
      this.coinPaymentCommand = coinPaymentCommand;
      this.productPaymentCommand = productPaymentCommand;

      this.paymentOrchestratorRepository = paymentOrchestratorRepository;
    }

    private int CalculateRemainder(List<CoinPaymentModel>
      coinsPayment, List<ProductPaymentModel> products)
    {
      var totalUserPayment = 0;

      foreach (var coinPayment in coinsPayment)
      {
        totalUserPayment += coinPayment.Value * coinPayment.Quantity;
      }

      var totalProductCost = 0;

      foreach (var product in products)
      {
        totalProductCost +=
          product.Quantity *
          paymentOrchestratorRepository.GetProductPrice(product.Name);
      }

      return totalUserPayment - totalProductCost;
    }

    public List<CoinPaymentModel> ProcessPayment(List<CoinPaymentModel>
      coinsPayment, List<ProductPaymentModel> products)
    {
      var errorMsg = productPaymentCommand.BuyProducts(products);

      if (string.IsNullOrEmpty(errorMsg))
      {
        var totalRemainder = CalculateRemainder(coinsPayment, products);

        if (totalRemainder < 0)
        {
          throw new Exception("Error in application layer: ProcessPayment.");
        }

        var paymentChange =
          coinPaymentCommand.ProcessCoins(coinsPayment, totalRemainder);

        productPaymentCommand.ConfirmBuyProducts(products);

        return paymentChange;

      } else
      {
        throw new Exception("Error in application layer: ProcessPayment.");
      }
    }
  }
}
