using VODB.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VODB.Tests.Models {
    public class Products : DbEntity {

        [DbIdentity]
        public int ProductId { get; set; }

        [DbRequired]
        public String ProductName { get; set; }

        [DbField("SupplierId")]
        public Suppliers Supplier {
            get { return GetValue<Suppliers>(); }
            set { SetValue(value); }
        }

        [DbField("CategoryId")]
        public Categories Category {
            get { return GetValue<Categories>(); }
            set { SetValue(value); }
        }

        public String QuantityPerUnit { get; set; }

        public Decimal UnitPrice { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Int16 UnitsOnOrder { get; set; }

        public Int16 ReorderLevel { get; set; }

        public Boolean Discontinued { get; set; }
        
    }
}
