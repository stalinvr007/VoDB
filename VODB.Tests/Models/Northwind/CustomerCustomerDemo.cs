using VODB.Annotations;

namespace VODB.Tests.Models.Northwind {
    public class CustomerCustomerDemo  {

        [DbKey("CustomerId")]
        public virtual Customers Customer { get; set; }

        [DbKey("CustomerTypeId")]
        public virtual CustomerDemographics Demographics { get; set; }

    }
}
