﻿using VODB.Annotations;
using System;
using System.Collections.Generic;
using VODB.DbLayer.DbResults;

namespace VODB.Tests.Models.Northwind
{

    [DbTable("Employees")]
    public class EmployeeNew
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
        public virtual EmployeeNew ReportsTo { get; set; }

        public String PhotoPath { get; set; }

        /// <summary>
        /// Gets a collection of employees that reports to this one.
        /// </summary>
        public virtual IEnumerable<EmployeeNew> ReportedFrom { get; set; }

        /// <summary>
        /// Gets the territories.
        /// </summary>
        public IEnumerable<EmployeeTerritories> Territories { get; set; }

    }
}
