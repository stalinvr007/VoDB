using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    public class Products {

        [DbIdentity]
        public int ProductId { get; set; }

        [DbRequired]
        public String ProductName { get; set; }

        [DbField("SupplierId")]
        public virtual Suppliers Supplier { get; set; }

        [DbField("CategoryId")]
        public virtual Categories Category { get; set; }

        public String QuantityPerUnit { get; set; }

        public Decimal UnitPrice { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Int16 UnitsOnOrder { get; set; }

        public Int16 ReorderLevel { get; set; }

        public Boolean Discontinued { get; set; }
        
    }
}
