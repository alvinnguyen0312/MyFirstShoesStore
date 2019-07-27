using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class ProductViewModel
    {
        public string BrandName { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int BrandID { get; set; }
        public string Id { get; set; }
        public string PRDName { get; set; }
        public string GRPName { get; set; }
        public float COSTPrice { get; set; }
        public float MSRP { get; set; }
        public int QTYOnHand { get; set; }
        public int QTYOnBackOrder { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
    }
}
