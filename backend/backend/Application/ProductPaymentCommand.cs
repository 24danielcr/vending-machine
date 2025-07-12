using System.Text.RegularExpressions;
using backend.Database;
using backend.Domain;
using backend.Infraestructure;

namespace backend.Application
{
  public class ProductPaymentCommand : IProductPaymentCommand
  {
    private readonly IProductPaymentRepository productPaymentRepository;
    public ProductPaymentCommand(IProductPaymentRepository
      productPaymentRepository)
    {
      this.productPaymentRepository = productPaymentRepository;
    }

    private void FormatProductName(ProductPaymentModel product)
    {
      product.Name = Regex.Replace(product.Name, " ", "");
    }

    private void FormatProductsName(List<ProductPaymentModel> products)
    {
      foreach (var product in products)
      {
        FormatProductName(product);
      }
    }
    private string ValidateProductsQuantity(List<ProductPaymentModel> products)
    {
      var errorMsg = string.Empty;

      foreach (var product in products)
      {
        var productQuantity
          = productPaymentRepository.GetProductQuantity(product.Name);

        if (productQuantity < product.Quantity) {
          errorMsg += "Insufficient " + product.Name + " quantity. ";
        }
      }

      return errorMsg;
    }

    public void ConfirmBuyProducts(List<ProductPaymentModel> products)
    {
      try
      {
        productPaymentRepository.BuyProducts(products);
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          "Error in application layer: ConfirmBuyProducts.", ex);
      }
    }

    public string BuyProducts(List<ProductPaymentModel> products)
    {
      var errorMsg = string.Empty;

      try
      {
        if (products != null)
        {
          FormatProductsName(products);
          errorMsg += ValidateProductsQuantity(products);
        }
        else
        {
          errorMsg += "No products were selected. ";
        }
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
          "Error in application layer: BuyProducts.", ex);
      }

      return errorMsg;
    }
  }
}
