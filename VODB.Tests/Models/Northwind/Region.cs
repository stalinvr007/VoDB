using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {

    public class Region {

        [DbKey("RegionID")]
        public int Id { get; set; }

        [DbField("RegionDescription")]
        public String Description { get; set; }

    }
}
