using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind
{
    public class Territories
    {

        [DbKey]
        public String TerritoryId { get; set; }

        [DbRequired]
        public String TerritoryDescription { get; set; }

        [DbField("RegionId")]
        public virtual Region Region { get; set; }

    }
}
