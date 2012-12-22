using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    public class Shippers {

        [DbIdentity]
        public int ShipperId { get; set; }

        [DbRequired]
        public String CompanyName { get; set; }

        public String Phone { get; set; }

    }
}
