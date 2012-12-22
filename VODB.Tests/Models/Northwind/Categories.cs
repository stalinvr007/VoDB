using System;
using VODB.Annotations;

namespace VODB.Tests.Models.Northwind
{
    
    public class Categories {

        [DbIdentity]
        public int CategoryId { get; set; }

        [DbRequired]
        public String CategoryName { get; set; }

        public String Description { get; set; }

    }
}
