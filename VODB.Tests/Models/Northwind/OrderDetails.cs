using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {

    [DbTable("Order Details")]
    public class OrderDetails : DbEntity {

        [DbKey("OrderId")]
        public Orders Order {
            get { return GetValue<Orders>(); }
            set { SetValue(value); }
        }

        [DbKey("ProductId")]
        public Products Product {
            get { return GetValue<Products>(); }
            set { SetValue(value); }
        }

        public Decimal UnitPrice { get; set; }

        public Int16 Quantity { get; set; }

        public Single Discount { get; set; }

    }
}
