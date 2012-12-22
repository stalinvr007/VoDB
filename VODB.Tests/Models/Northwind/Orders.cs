using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    
    public class Orders {

        [DbIdentity]
        public int OrderId { get; set; }

        [DbField("CustomerId")]
        public virtual Customers Customer { get; set; }

        [DbField("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        [DbField("ShipVia"), DbBind("ShipperId")]
        public virtual Shippers Shipper { get; set; }
        
        public Decimal Freight { get; set; }

        public String ShipName { get; set; }
        
        public String ShipAddress { get; set; }

        public String ShipCity { get; set; }

        public String ShipRegion { get; set; }

        public String ShipPostalCode { get; set; }

        public String ShipCountry { get; set; }


    }
}
