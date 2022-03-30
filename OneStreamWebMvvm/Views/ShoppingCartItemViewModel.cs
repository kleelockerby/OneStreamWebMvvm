using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
	public class ShoppingCartItemViewModel : ViewModelBase
	{
		private CartItemModel cartItemModel;
		private ProductModel? productModel;

		private int? cartID;
		public int? CartID { get => cartID; set { SetProperty(ref cartID, value, nameof(CartID)); } }

		private int? productID;
		public int? ProductID { get => productID; set { SetProperty(ref productID, value, nameof(ProductID)); } }

		private string? name;
		public string? Name { get => name; set { SetProperty(ref name, value, nameof(Name)); } }

		private decimal? quantity;
		public decimal? Quantity { get => quantity; set { SetProperty(ref quantity, value, nameof(Quantity)); } }

		private DateTime orderDate;
		public DateTime OrderDate { get => orderDate; set { SetProperty(ref orderDate, value, nameof(OrderDate)); } }

		private decimal? price;
		public decimal? Price { get => price; set { SetProperty(ref price, value, nameof(Price)); } }

		//public decimal? Total { get => cartItemModel.Total; }

		public ShoppingCartItemViewModel(CartItemModel CartItemModel, decimal? price)
		{
			this.cartItemModel = CartItemModel;
			this.cartID =cartItemModel.CartID;
			this.productID = cartItemModel.ProductID;
			this.name = cartItemModel.Name;
			this.quantity = cartItemModel.Quantity;
			this.orderDate = cartItemModel.OrderDate;
			this.price = price;
		}
	}
}
