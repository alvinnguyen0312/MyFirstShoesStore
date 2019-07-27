using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class BrandViewModel
    {
        public BrandViewModel() { }
        public string BrandName { get; set; }
        public int Id { get; set; }
        public string selectedId { get; set; }
        public List<Brand> Brands { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return Brands.Select(brand => new SelectListItem
            {
                Text = brand.Name,
                Value = brand.Id.ToString()
            });
        }
        public IEnumerable<Product> Products { get; set; }
        public void SetBrands(List<Brand>brds)
        {
            Brands = brds;
        }
        public int Qty { get; set; }
        public int BrandId { get; set; }

    }
}
