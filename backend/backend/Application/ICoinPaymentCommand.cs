using backend.Domain;

namespace backend.Application
{
  public interface ICoinPaymentCommand
  {
    public List<CoinPaymentModel> ProcessCoins(
     List<CoinPaymentModel> coinPaymentModels, int totalRemainder);
  }
}
