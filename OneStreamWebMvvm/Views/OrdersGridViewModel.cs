using Microsoft.AspNetCore.Components;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrdersGridViewModel : ViewModelBase
    {
        private readonly IOrdersService ordersService;

        public OrderViewModel? SelectedViewModelOrder;
        private ViewModelCollectionBase<OrderViewModel> viewModelOrders;
        public ViewModelCollectionBase<OrderViewModel> ViewModelOrders
        {
            get => viewModelOrders;
            set
            {
                SetProperty(ref viewModelOrders, value, nameof(ViewModelOrders));
            }
        }

        public OrdersGridViewModel(IOrdersService OrdersService)
        {
            this.ordersService = OrdersService;
        }

        protected override async Task OnInitializedAsync()
        {
            IEnumerable<OrderModel> orderModels = await ordersService?.GetOrderModels()!;
            IEnumerable<OrderViewModel> orderViewModels = orderModels.Select(x => new OrderViewModel(x));
            this.viewModelOrders = new ViewModelCollectionBase<OrderViewModel>(orderViewModels);
        }

        public void AddRecord()
        {
            OrderModel newOrderModel = new OrderModel(10011, new DateTime(2017, 05, 15), "FREDB", "Fred's Bicycles");
            OrderViewModel newOrderViewModel = new OrderViewModel(newOrderModel);
            ViewModelOrders.Add(newOrderViewModel);
            this.SelectedViewModelOrder = newOrderViewModel;
        }

        public void DeleteRecord()
        {
            if (ViewModelOrders.Count() != 0)
            {
                OrderViewModel orderViewModel = this.ViewModelOrders[4];
                ViewModelOrders.Remove(orderViewModel);
            }
        }

        public void UpdateRecord()
        {
            this.SelectedViewModelOrder = this.ViewModelOrders[2];
            OrderViewModel orderViewModel = this.SelectedViewModelOrder;
            orderViewModel.CustomerID = "BNESEN";
            orderViewModel.CustomerName = "Bon Nese app";
        }
    }
}