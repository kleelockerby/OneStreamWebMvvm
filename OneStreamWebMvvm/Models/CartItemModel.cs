namespace OneStreamWebMvvm
{
	public class CartItemModel
	{
		private int? cartID;
		private int? productID;
		private string? name;
		private int? quantity;
		private DateTime orderDate;

		public int? CartID => cartID;
		public int? ProductID => productID;
		public string? Name => name;
		public int? Quantity => quantity;

		public DateTime OrderDate => orderDate;
		
		//public ProductModel Product { get; set; }

		/*private decimal? quantity;
		public decimal? Quantity
		{
			get => quantity;
			set
			{
				this.quantity = value;
				if (!Equals(quantity, value))
				{
					NotifyModelChanged();
				}
			}
		}*/

		public event Action? ModelChanged;
		private void NotifyModelChanged() => ModelChanged?.Invoke();

		//public decimal? Total { get { return (decimal?)Quantity * Product.Price; } }

		public CartItemModel(int? cartID, int? productID, string? name, int? quantity, DateTime orderDate)
		{
			this.cartID = cartID;
			this.productID = productID;
			this.name = name;
			this.quantity = quantity;
			this.orderDate = orderDate;
		}
	}
}
