namespace OneStreamWebMvvm
{
    public class CartItem
    {
        public int? CartID { get; set; }
        public int? ProductID { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public DateTime OrderDate { get; set; }

        public CartItem() { }
        public CartItem(int? cartID, int? productID, string? name, int? quantity, DateTime orderDate)
        {
            this.CartID = cartID;
            this.ProductID = productID;
            this.Name = name;
            this.Quantity = quantity;
            this.OrderDate = orderDate;
        }
    }
}
