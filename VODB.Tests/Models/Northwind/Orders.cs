using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    
    public sealed class Orders : DbEntity {

        [DbIdentity]
        public int OrderId { get; set; }

        [DbField("CustomerId")]
        public Customers Customer {
            get { 
                return GetValue<Customers>(); 
            }
            set { SetValue(value); }
        }

        [DbField("EmployeeId")]
        public Employees Employee {
            get { return GetValue<Employees>(); }
            set { SetValue(value); }
        }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        [DbField("ShipVia"), DbBind("ShipperId")]
        public Shippers Shipper {
            get { return GetValue<Shippers>(); }
            set { SetValue(value); }
        }
        
        public Decimal Freight { get; set; }

        public String ShipName { get; set; }
        
        public String ShipAddress { get; set; }

        public String ShipCity { get; set; }

        public String ShipRegion { get; set; }

        public String ShipPostalCode { get; set; }

        public String ShipCountry { get; set; }


    }
}
