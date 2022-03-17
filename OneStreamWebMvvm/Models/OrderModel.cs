using OneStreamWebUI.Mvvm.Toolkit;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OneStreamWebMvvm
{
    public class OrderModel : ModelBase
    {
        private int orderID;
        private DateTime orderDate;
        private string? customerID;
        private string? customerName;

        public int OrderID => orderID;
        public DateTime OrderDate => orderDate;

        public string? CustomerID
        {
            get => customerID;
            set { SetProperty(ref customerID, value, nameof(CustomerID)); }
        }

        public string? CustomerName
        {
            get => customerName;
            set { SetProperty(ref customerName, value, nameof(CustomerName)); }
        }

        public OrderModel(int orderID, DateTime orderDate, string customerID, string customerName)
        {
            this.orderID = orderID;
            this.orderDate = orderDate;
            this.CustomerID = customerID;
            this.CustomerName = customerName;
        }
    }
}
