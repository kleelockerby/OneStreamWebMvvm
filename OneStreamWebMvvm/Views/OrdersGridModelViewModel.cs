using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrdersGridModelViewModel : ViewModelBase
    {
        private readonly IOrdersService ordersService;

        public OrderModel? SelectedModelOrder;
        private ViewModelCollection<OrderModel> viewOrders;
        public ViewModelCollection<OrderModel> ViewOrders
        {
            get => viewOrders;
            set
            {
                SetProperty(ref viewOrders, value, nameof(ViewOrders));
            }
        }

        public OrdersGridModelViewModel(IOrdersService OrdersService)
        {
            this.ordersService = OrdersService;
        }

        protected override async Task OnInitializedAsync()
        {
            IEnumerable<OrderModel> orderModels = await ordersService?.GetOrders()!;
            this.viewOrders = new ViewModelCollection<OrderModel>(orderModels);
        }

        public void AddRecord()
        {
            OrderModel newOrderModel = new OrderModel(10011, new DateTime(2017, 05, 15), "FREDB", "Fred's Bicycles");
            ViewOrders.Add(newOrderModel);
            this.SelectedModelOrder = newOrderModel;
        }

        public void DeleteRecord()
        {
            if (ViewOrders.Count() != 0)
            {
                OrderModel orderModel = this.ViewOrders[4];
                ViewOrders.Remove(orderModel);
            }
        }

        public void UpdateRecord()
        {
            this.SelectedModelOrder = this.ViewOrders[2];
            OrderModel orderModel = this.SelectedModelOrder;
            orderModel.CustomerID = "BNESEN";
            orderModel.CustomerName = "Bon Nese app";
        }
    }
}