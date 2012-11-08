using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;


namespace VODB.Tests.Models {
    public class CustomerDemographics : DbEntity{

        [DbKey]
        public String CustomerTypeId { get; set; }

        public String CustomerDesc { get; set; }

    }
}
