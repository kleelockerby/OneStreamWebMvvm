namespace OneStreamWebMvvm
{
    public interface ICartItemModel
    {
		public int? CartID { get; }
		public int? ProductID { get; }
		public string? Name { get; }
		public int? Quantity { get; }
		public DateTime OrderDate { get; }
		public string QuantityString { get; set; }
		public ProductModel? Product { get; set; }
		public decimal? Total { get; }

		/*public int? ProductID { get; set; }
		public string? Name { get; set; }
		public int? Quantity { get; set; }
		public DateTime OrderDate { get; set; }
		public string QuantityString { get; set; }
		public ProductModel? Product { get; set; }
		public decimal? Total { get; set; }*/
	}
}
