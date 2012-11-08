
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB;
using VODB.Annotations;


namespace VODB.Tests.Models
{
    
    public class Categories : DbEntity {

        [DbIdentity]
        public int CategoryId { get; set; }

        [DbRequired]
        public String CategoryName { get; set; }

        public String Description { get; set; }

    }
}
