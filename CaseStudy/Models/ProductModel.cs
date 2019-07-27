using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class ProductModel
    {
        private AppDbContext _db;
        public ProductModel(AppDbContext context)
        {
            _db = context;
        }
        public List<Product> GetAll()
        {
            return _db.Products.ToList();
        }
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(item => item.Brand.Id == id).ToList();
        }
        public Brand GetBrand(int id)
        {
            return _db.Brands.FirstOrDefault(br => br.Id == id);
        }
    }
}
