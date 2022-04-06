using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
	public class CartModel
	{
		public ViewModelCollectionBase<CartItemModel> Items { get; set; } = new ViewModelCollectionBase<CartItemModel>();
        
        //public event Action? ModelChanged;
		//private void NotifyModelChanged() => ModelChanged?.Invoke();

        public decimal? Total
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
        }

        public CartModel()
        {
            this.Items = new ViewModelCollectionBase<CartItemModel>();
        }
    }
}
