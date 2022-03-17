using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrdersViewModel : ViewModelCollectionBase<OrderViewModel>
    {
        private readonly IOrdersService ordersService;

        public List<OrderViewModel> ViewModelOrders { get => List; }
        public OrderViewModel? SelectedViewModelOrder;

        public OrdersViewModel(IOrdersService OrdersService)
        {
            this.ordersService = OrdersService;            
        }

        protected override async Task OnInitializedAsync()
        {
            IEnumerable<OrderModel> orders = await ordersService?.GetOrders()!;
            IEnumerable<OrderViewModel> orderViewModels = orders.Select(x => new OrderViewModel(x));
            InitializeList(orderViewModels);
        }

        public void UpdateOrder()
        {
            this.SelectedViewModelOrder = this.ViewModelOrders[2];
            OrderViewModel orderViewModel = this.SelectedViewModelOrder;
            orderViewModel.CustomerID = "BNESEN";
            orderViewModel.CustomerName = "Bon Nese app";
        }

        public void AddOrder()
        {
            OrderModel newOrderModel = new OrderModel(10011, new DateTime(2017, 05, 15), "FREDB", "Fred's Bicycles");
            OrderViewModel newOrderViewModel = new OrderViewModel(newOrderModel);
            this.Add(newOrderViewModel);
            this.SelectedViewModelOrder = newOrderViewModel;
        }

        public void DeleteOrder()
        {
            OrderViewModel orderViewModel = this.ViewModelOrders[4];
            this.Remove(orderViewModel);
        }

        public void OnRowClick(OrderViewModel viewModel)
        {
            this.SelectedViewModelOrder = viewModel;
        }
    }
}