using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrdersComponentViewModel : ViewModelBase
    {
        [Parameter] public List<OrderModel>? DataSource { get; set; }

        public OrderComponentViewModel? SelectedViewModelOrder;

        private ViewModelCollection<OrderComponentViewModel>? viewModelOrders;
        public ViewModelCollection<OrderComponentViewModel>? ViewModelOrders  { get => viewModelOrders; set => SetProperty(ref viewModelOrders, value, nameof(ViewModelOrders)); }

        public override void OnParametersSet()
        {
            List<OrderComponentViewModel> ViewModelOrdersList = new List<OrderComponentViewModel>(DataSource.Select(x => new OrderComponentViewModel(x)));
            this.ViewModelOrders = new ViewModelCollection<OrderComponentViewModel>(ViewModelOrdersList);
        }

        public void AddOrder()
        {
            OrderModel newOrderModel = new OrderModel(10011, new DateTime(2017, 05, 15), "FREDB", "Fred's Bicycles");
            OrderComponentViewModel newOrderViewModel = new OrderComponentViewModel(newOrderModel);
            ViewModelOrders?.Add(newOrderViewModel);
            this.SelectedViewModelOrder = newOrderViewModel;
        }

        public void DeleteOrder()
        {
            if (ViewModelOrders?.Count() != 0)
            {
                OrderComponentViewModel? orderViewModel = this.ViewModelOrders?[4];
                ViewModelOrders?.Remove(orderViewModel!);
            }
        }

        public void UpdateOrder()
        {
            this.SelectedViewModelOrder = this.ViewModelOrders?[2];
            OrderComponentViewModel? orderViewModel = this.SelectedViewModelOrder;
            orderViewModel.CustomerID = "BNESEN";
            orderViewModel.CustomerName = "Bon Nese app";
        }
    }
}