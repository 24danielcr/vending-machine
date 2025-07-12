using backend.Database;
using backend.Domain;

namespace backend.Infraestructure
{
	public class ProductPaymentRepository : IProductPaymentRepository
	{
		private readonly VendingMachineDB vendingMachineDB;

		public ProductPaymentRepository(VendingMachineDB vendingMachineDB)
		{
			this.vendingMachineDB = vendingMachineDB;
		}

		public void BuyProducts(List<ProductPaymentModel> products)
		{
			var vendingMachineProducts = vendingMachineDB.vendingMachineProducts;

			try
			{
				foreach (var product in products)
				{
					if (Enum.TryParse<Product>
						(product.Name, true, out var productName))
					{
						vendingMachineProducts[productName].Quantity -= product.Quantity;
          }
					else
					{
            throw new Exception(
							"Error in repository layer: GetAllProducts.");
          }
				}
			}
			catch (Exception)
			{
        throw;
      }
		}

		public int GetProductQuantity(string productName)
		{
      var vendingMachineProducts = vendingMachineDB.vendingMachineProducts;
      if (Enum.TryParse<Product>
            (productName, true, out var productEnum))
			{
				return vendingMachineProducts[productEnum].Quantity;
			}
      else
      {
        throw new Exception(
          "Error in repository layer: GetProductQuantity.");
      }
    }
	}
}