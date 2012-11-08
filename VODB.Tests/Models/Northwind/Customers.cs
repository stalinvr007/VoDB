using VODB.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VODB.Tests.Models {
    public class Customers : DbEntity {

        [DbKey]
        public String CustomerId { get; set; }

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

    }
}
