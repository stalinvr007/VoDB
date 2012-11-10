using VODB.Annotations;
using System;

namespace VODB.Tests.Models.Northwind {
    public class Shippers : DbEntity {

        [DbIdentity]
        public int ShipperId { get; set; }

        [DbRequired]
        public String CompanyName { get; set; }

        public String Phone { get; set; }

    }
}
