namespace OneStreamWebMvvm
{
	public class CartItemModel
	{
		private int? cartID;
		private int? productID;
		private string? name;
		private DateTime orderDate;

		public int? CartID { get; set; }
		public int? ProductID { get; set; }
		public string? Name { get; set; }
		public DateTime OrderDate { get; set; }

		private int? quantity;
		public int? Quantity
		{
			get => quantity;
			set
			{
				this.quantity = value;
			}
		}

		public string QuantityString { get => Quantity?.ToString(); set { quantity = Int32.Parse(value); } }

		public ProductModel? Product { get; set; }

        public decimal? Total
        {
            get { return (Product != null) ? (decimal?)Quantity * Product?.Price : 0.0M; }

        }

		public CartItemModel() { }

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
