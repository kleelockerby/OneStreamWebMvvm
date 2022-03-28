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
        private readonly IOrdersService? ordersService;
        public List<OrderModel> Items = new List<OrderModel>();

        public OrdersGridViewModel(IOrdersService OrdersService)
        {
            this.ordersService = OrdersService;
        }

        public override async Task OnInitializedAsync()
        {
            IEnumerable<OrderModel> orderModels = await ordersService?.GetOrderModels()!;
            this.Items = orderModels.ToList();
        }
    }
}