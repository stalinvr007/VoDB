using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;


namespace VODB.Tests.Models {
    public class CustomerCustomerDemo : DbEntity {

        [DbKey("CustomerId")]
        public Customers Customer { 
            get { return GetValue<Customers>(); }
            set { SetValue(value); }
        }

        [DbKey("CustomerTypeId")]
        public CustomerDemographics Demographics {
            get { return GetValue<CustomerDemographics>(); }
            set { SetValue(value); }
        }

    }
}
