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

#nullable disable
		public string QuantityString { get => Quantity?.ToString(); set { quantity = Int32.Parse(value); } }

		public ProductModel? Product { get; set; }

		public event Action? ModelChanged;
		private void NotifyModelChanged() => ModelChanged?.Invoke();

        public decimal? Total
        {
            get { return (Product != null) ? (decimal?)Quantity * Product?.Price : 0.0M; }

        }

        public CartItemModel(int? cartID, int? productID, string? name, int? quantity, DateTime orderDate)
		{
			this.cartID = cartID;
			this.productID = productID;
			this.name = name;
			this.quantity = quantity;
			this.orderDate = orderDate;
			this.Product = new ProductModel();
		}
	}
}
