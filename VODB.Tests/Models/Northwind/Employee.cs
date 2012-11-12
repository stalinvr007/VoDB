using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {

    /// <summary>
    /// 
    /// </summary>
    [DbTable("Employees")]
    public sealed class Employee : DbEntity {

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
        public Employee ReportsTo {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }

        public String PhotoPath { get; set; }
        

    }
}
