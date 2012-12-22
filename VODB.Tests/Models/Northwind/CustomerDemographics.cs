using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {
    public class CustomerDemographics {

        [DbKey]
        public String CustomerTypeId { get; set; }

        public String CustomerDesc { get; set; }

    }
}
