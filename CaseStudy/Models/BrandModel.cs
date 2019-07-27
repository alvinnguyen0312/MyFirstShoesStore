using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class BrandModel
    {
        private AppDbContext _db;
        public BrandModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Brand> GetAll()
        {
            return _db.Brands.ToList<Brand>();
        }
        public string GetName(int id)
        {
            Brand br = _db.Brands.First(b => b.Id == id);
            return br.Name;
        }

        public Product GetByProductID(string id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
