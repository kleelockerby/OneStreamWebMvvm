
namespace OneStreamWebMvvm
{
	public class CartModel
	{
		public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();

       /* public decimal? Total
        {
            get
            {
                decimal? total = 0;
                foreach (var item in Items)
                {
                    total += item.Total;
                }
                return total;
            }
        }*/
    }
}
