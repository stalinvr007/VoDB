using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;

namespace VODB.Tests.Models {

    public class Region : DbEntity {

        [DbKey("RegionID")]
        public int Id { get; set; }

        [DbField("RegionDescription")]
        public String Description { get; set; }

    }
}
