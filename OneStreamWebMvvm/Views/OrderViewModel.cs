using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrderViewModel : ViewModelBase
    {
        public OrderModel Model { get; set; }

        public int OrderID => Model.OrderID;
        public DateTime OrderDate => Model.OrderDate;

        public string? CustomerID
        {
            get => Model.CustomerName;
            set { SetProperty(value, ((customerName) => Model.CustomerName = customerName!), nameof(CustomerName)); }
        }

        public string? CustomerName
        {
            get => Model.CustomerName;
            set { SetProperty(value, ((customerName) => Model.CustomerName = customerName!), nameof(CustomerName)); }
        }

        public OrderViewModel(OrderModel orderModel)
        {
            this.Model = orderModel;
        }
    }
}
