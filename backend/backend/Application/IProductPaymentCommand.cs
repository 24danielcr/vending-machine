using backend.Domain;

namespace backend.Application
{
  public interface IProductPaymentCommand
  {
    public string BuyProducts(List<ProductPaymentModel> products);

    public void ConfirmBuyProducts(List<ProductPaymentModel> products);
  }
}
