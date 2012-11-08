using VODB.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VODB.Tests.Models {

    [DbTable("Order Details")]
    public class OrderDetails : DbEntity {

        [DbField("OrderId"), DbKey]
        public Orders Order {
            get { return GetValue<Orders>(); }
            set { SetValue(value); }
        }

        [DbField("ProductId"), DbKey]
        public Products Product {
            get { return GetValue<Products>(); }
            set { SetValue(value); }
        }

        public Decimal UnitPrice { get; set; }

        public Int16 Quantity { get; set; }

        public Single Discount { get; set; }

    }
}
