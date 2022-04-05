namespace OneStreamWebMvvm
{
	public class ProductModel
	{
		public int? ProductID { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }

		public ProductModel() { }
		public ProductModel(int? productID, string? name, string? description, decimal? price)
		{
			this.ProductID = productID;
			this.Name = name;
			this.Description = description;
			this.Price = price;
		}
	}
}
