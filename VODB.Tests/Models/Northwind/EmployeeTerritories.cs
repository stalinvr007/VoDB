using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind
{
    public class EmployeeTerritories
    {
        [DbKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [DbKey("TerritoryID")]
        public virtual Territories Territories { get; set; }

    }
}
