using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrderViewModel : ViewModelBase
    {
        public Order Model { get; set; }
        
        public int OrderID => Model.OrderID;
        public DateTime OrderDate => Model.OrderDate;

        public string? CustomerID
        {
            get => Model.CustomerID;
            set { SetProperty(value, ((customerID) => Model.CustomerID = customerID!), nameof(CustomerID)); }
        }

        public string? CustomerName
        {
            get => Model.CustomerName;
            set { SetProperty(value, ((customerName) => Model.CustomerName = customerName!), nameof(CustomerName));}           
        }

        public OrderViewModel(Order orderModel)
        {
            this.Model = orderModel;
        }
    }
}
