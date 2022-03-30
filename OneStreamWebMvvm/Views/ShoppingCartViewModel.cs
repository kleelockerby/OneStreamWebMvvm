using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
#nullable disable
	public class ShoppingCartViewModel : ViewModelBase
	{
		private readonly ICartItemService? CartItemService;
		private readonly IProductService? ProductService;

		public CartModel ShoppingCart { get; set; } = new CartModel();
		public CartItemModel? SelectedCartItemModel { get; set; }
		public SpinnerComponent? SpinnerRef { get; set; }

		private ViewModelCollectionBase<ShoppingCartItemViewModel>? cartItems;
		public ViewModelCollectionBase<ShoppingCartItemViewModel>? CartItems
		{
			get => cartItems;
			set
			{
				SetProperty(ref cartItems, value, nameof(CartItems));
			}
		}

		public ShoppingCartViewModel(ICartItemService? cartItemService, IProductService? productService)
		{
			this.CartItemService = cartItemService;
			this.ProductService = productService;
			SpinnerRef?.Show();
		}

		public override async Task OnInitializedAsync()
		{
			IEnumerable<CartItemModel> cartModelItems = await CartItemService?.GetCartItemModels()!;
			IEnumerable<ProductModel> productModels = await ProductService.GetProductModels()!;

			this.cartItems = new ViewModelCollectionBase<ShoppingCartItemViewModel>();

			foreach (CartItemModel cartItemModel in cartModelItems)
			{
				ProductModel product = productModels.Where(p => p.ProductID == cartItemModel.ProductID).FirstOrDefault();
				ShoppingCartItemViewModel viewItemModel = new ShoppingCartItemViewModel(cartItemModel, product.Price);
				this.cartItems.Add(viewItemModel);
			}
			SpinnerRef?.Hide();
		}
	}
}