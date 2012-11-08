using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;

namespace VODB.Tests.Models {
    
    public class EmployeeTerritories : DbEntity {

        [DbKey("EmployeeId")]
        public Employees Employee {
            get { return GetValue<Employees>(); }
            set { SetValue(value); }
        }

        [DbKey("TerritoryID")]
        public Territories Territory {
            get { return GetValue<Territories>(); }
            set { SetValue(value); }
        }

    }
}
