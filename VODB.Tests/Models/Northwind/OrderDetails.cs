using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {

    [DbTable("Order Details")]
    public class OrderDetails  {

        [DbKey("OrderId")]
        public virtual Orders Order { get; set; }

        [DbKey("ProductId")]
        public virtual Products Product { get; set; }

        public Decimal UnitPrice { get; set; }

        public Int16 Quantity { get; set; }

        public Single Discount { get; set; }

    }
}
