using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {
    public class CustomerDemographics : DbEntity{

        [DbKey]
        public String CustomerTypeId { get; set; }

        public String CustomerDesc { get; set; }

    }
}
