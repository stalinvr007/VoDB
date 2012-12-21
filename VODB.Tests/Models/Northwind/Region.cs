using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind
{
    public class Region
    {
        [DbKey]
        public int RegionId { get; set; }

        public String RegionDescription { get; set; }

    }
}
