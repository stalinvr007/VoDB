using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {

    public class Territories : DbEntity {

        [DbKey("TerritoryID")]
        public String Id { get; set; }

        [DbField("TerritoryDescription"), DbRequired]
        public String Description { get; set; }

        [DbField("RegionId")]
        public Region Region {
            get { return GetValue<Region>(); }
            set { SetValue(value); }
        }

    }
}