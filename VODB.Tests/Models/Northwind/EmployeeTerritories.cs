using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {
    
    public class EmployeeTerritories : DbEntity {

        [DbKey("EmployeeId")]
        public Employee Employee {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }

        [DbKey("TerritoryID")]
        public Territories Territory {
            get { return GetValue<Territories>(); }
            set { SetValue(value); }
        }

    }
}
