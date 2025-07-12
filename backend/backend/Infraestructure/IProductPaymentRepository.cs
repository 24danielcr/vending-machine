using backend.Domain;

namespace backend.Infraestructure
{
  public interface IProductPaymentRepository
  {
    public void BuyProducts(List<ProductPaymentModel> products);

    public int GetProductQuantity(string productName);
  }
}
