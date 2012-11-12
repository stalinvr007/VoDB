using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {
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
