﻿using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    public class Suppliers {

        [DbIdentity]
        public int SupplierId { get; set; }

        [DbRequired]
        public String CompanyName { get; set; }

        public String ContactName { get; set; }

        public String ContactTitle { get; set; }

        public String Address { get; set; }

        public String City { get; set; }

        public String Region { get; set; }

        public String PostalCode { get; set; }

        public String Country { get; set; }

        public String Phone { get; set; }

        public String Fax { get; set; }

        public String HomePage { get; set; }

    }
}
