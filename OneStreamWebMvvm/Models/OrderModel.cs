using OneStreamWebUI.Mvvm.Toolkit;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OneStreamWebMvvm
{
    public class OrderModel
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
            set { customerID = value; }
        }

        public string? CustomerName
        {
            get => customerName;
            set { customerName = value; }
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
