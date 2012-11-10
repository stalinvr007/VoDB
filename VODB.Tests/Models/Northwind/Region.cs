using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {

    public class Region : DbEntity {

        [DbKey("RegionID")]
        public int Id { get; set; }

        [DbField("RegionDescription")]
        public String Description { get; set; }

    }
}
