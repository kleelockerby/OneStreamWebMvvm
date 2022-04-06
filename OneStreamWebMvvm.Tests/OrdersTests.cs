using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using OneStreamWebMvvm;
using System.Linq;

namespace OneStreamWebMvvm.Tests
{
    public class OrdersTests
    {
        //private IProductRepository ProductRepository;
       //private List<ProductModel>? products;
        private Mock<ICartItemService> cartItemServiceMock;
        private List<CartItemModel>? cartItems;
        private ShoppingCartViewModel? shoppingCartViewModel;

        public OrdersTests()                //public OrdersTests(IProductRepository productRepository)
        {
            //this.ProductRepository = productRepository;
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();

            this.cartItemServiceMock = new Mock<ICartItemService>();
            Mock<CartItemModel> cartItemModelMock = new Mock<CartItemModel>();
            
            cartItems = new List<CartItemModel>() { cartItemModelMock.Object };
            
            this.cartItemServiceMock.Setup(repo => repo.GetCartItemModels()).Returns((Task<IEnumerable<CartItemModel>>)cartItems.AsEnumerable());

            IList<ProductModel> productDetails = new List<ProductModel>()
            {
                new ProductModel()
                {
                   ProductID = 98,
                   Name = "Some Fake Product",
                   Description = "Some Fake Product Description",
                   Price = 9.99M
                },
                new ProductModel()
                {
                   ProductID = 99,
                   Name = "Another Fake Product",
                   Description = "Another Fake Product Description",
                   Price = 8.99M
                 }
            };
            productRepositoryMock.Setup(repo => repo.GetProducts()).Returns((Task<IEnumerable<ProductModel>>)productDetails.AsEnumerable());
            shoppingCartViewModel = new ShoppingCartViewModel(cartItemServiceMock.Object, productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllProducts()
        {
            int expected = 2;
            var products = await shoppingCartViewModel?.GetProductRepository()!;
            var productsList = products.ToList();
            Assert.Equal(expected, productsList.Count());
        }
    }
}
