using System.ComponentModel;
using System.Runtime.CompilerServices;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class OrderModel : ModelBase
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }

        public OrderModel() { }

        public OrderModel(int orderID, DateTime orderDate, string customerID, string customerName)
        {
            this.OrderID = orderID;
            this.OrderDate = orderDate;
            this.CustomerID = customerID;
            this.CustomerName = customerName;
        }
    }
}
