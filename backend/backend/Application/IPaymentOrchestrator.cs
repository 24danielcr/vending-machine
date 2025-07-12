using backend.Domain;

namespace backend.Application
{
  public interface IPaymentOrchestrator
  {
    public List<CoinPaymentModel> ProcessPayment(List<CoinPaymentModel>
      coinsPayment, List<ProductPaymentModel> products);
  }
}
