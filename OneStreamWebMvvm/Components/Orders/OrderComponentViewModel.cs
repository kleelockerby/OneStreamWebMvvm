using System;
using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrderComponentViewModel : ViewModelBase
    {
        private OrderModel? orderModel;
        private int? orderID;
        private DateTime orderDate;
        private string customerID;
        private string customerName;

        public int? OrderID { get => orderID; }
        public DateTime OrderDate { get => orderDate; set { SetProperty(ref orderDate, value, nameof(OrderDate)); } }
        public string? CustomerID { get => customerID; set { SetProperty(ref customerID, value, nameof(CustomerID)); } }
        public string? CustomerName { get => customerName; set { SetProperty(ref customerName, value, nameof(CustomerName)); } }

        public OrderModel? OrderModel { get => orderModel; set => orderModel = value; }

        public OrderComponentViewModel(OrderModel orderModel)
        {
            this.orderModel = orderModel;
            this.orderID = this.orderModel.OrderID;
            this.orderDate = this.orderModel.OrderDate;
            this.customerID = this.orderModel?.CustomerID!;
            this.customerName = this.orderModel?.CustomerName!;
        }
    }
}
