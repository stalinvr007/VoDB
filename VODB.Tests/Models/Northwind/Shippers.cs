using VODB.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VODB.Tests.Models {
    public class Shippers : DbEntity {

        [DbIdentity]
        public int ShipperId { get; set; }

        [DbRequired]
        public String CompanyName { get; set; }

        public String Phone { get; set; }

    }
}
