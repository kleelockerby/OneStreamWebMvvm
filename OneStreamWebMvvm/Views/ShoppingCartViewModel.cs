using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
#nullable disable
	public class ShoppingCartViewModel : ViewModelBase
	{
		private readonly ICartItemService CartItemService;
		//private readonly IProductService ProductService;
		private readonly IProductRepository ProductRepository;

		public ViewModelCollectionBase<ShoppingCartItemViewModel> ProductItems { get; private set; }
		public ViewModelCollectionBase<CartItemModel> Items { get => ShoppingCart.Items; }
		public CartModel ShoppingCart { get; set; }

		public ShoppingCartViewModel(ICartItemService cartItemService, IProductRepository productRepository)
		{
			this.CartItemService = cartItemService;
			this.ProductRepository = productRepository;
		}

		public override async Task OnInitializedAsync()
		{
			IEnumerable<CartItemModel> cartModelItems = await CartItemService.GetCartItemModels()!;
			//IEnumerable<ProductModel> productModels = await ProductService.GetProductModels()!;
			IEnumerable<ProductModel> productModels = await GetProductRepository();

			this.ShoppingCart = new CartModel();
			this.ProductItems = new ViewModelCollectionBase<ShoppingCartItemViewModel>();

			foreach (CartItemModel cartItemModel in cartModelItems)
			{
				ProductModel product = productModels.Where(p => p.ProductID == cartItemModel.ProductID).FirstOrDefault();
				cartItemModel.Product = product;
				ShoppingCartItemViewModel viewItemModel = new ShoppingCartItemViewModel(cartItemModel);
				this.ProductItems.Add(viewItemModel);
			}
		}

		public async Task<IEnumerable<ProductModel>> GetProductRepository()
        {
			IEnumerable<ProductModel> products = await ProductRepository.GetProducts();
			return products;

		}

		public void OnButtonClick(ShoppingCartItemViewModel cartItemViewModel)
        {
			AddCartitem(cartItemViewModel);
        }

		public void AddCartitem(ShoppingCartItemViewModel cartItemViewModel)
		{
			this.ProductItems.Remove(cartItemViewModel);
			this.ShoppingCart.Items.Add(cartItemViewModel.CartItemModel);
		}
	}
}