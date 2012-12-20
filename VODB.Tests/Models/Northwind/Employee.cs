using VODB.Annotations;
using System;
using System.Collections.Generic;
using VODB.DbLayer.DbResults;

namespace VODB.Tests.Models.Northwind
{
    [DbTable("Employees")]
    public class Employee
    {

        [DbIdentity]
        public int EmployeeId { get; set; }

        [DbRequired]
        public String LastName { get; set; }

        [DbRequired]
        public String FirstName { get; set; }

        public String Title { get; set; }

        public String TitleOfCourtesy { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public String Address { get; set; }

        public String City { get; set; }

        public String Region { get; set; }

        public String PostalCode { get; set; }

        public String Country { get; set; }

        public String HomePhone { get; set; }

        public String Extension { get; set; }

        public String Notes { get; set; }

        public Byte[] Photo { get; set; }

        [DbBind("EmployeeId")]
        public virtual Employee ReportsTo { get; set; }

        public String PhotoPath { get; set; }

        public virtual IEnumerable<Employee> ReportedFrom { get; set; }

        //public virtual IEnumerable<EmployeeTerritories> Territories { get; set; }

        [DbIgnore]
        public bool NonExistingField { get; set; }
    }
}
