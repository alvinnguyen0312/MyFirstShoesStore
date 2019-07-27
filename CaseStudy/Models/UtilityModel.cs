using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class UtilityModel
    {
        private AppDbContext _db;
        public UtilityModel(AppDbContext context) // will be sent by controller
        {
            _db = context;
        }

        public bool loadShoesTables(string stringJson)
        {
            bool brandLoaded = false;
            bool productLoaded = false;
            try
            {
                dynamic objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject(stringJson);
                brandLoaded = loadBrands(objectJson);
                productLoaded = loadProducts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandLoaded && productLoaded;
        }

        private bool loadBrands(dynamic objectJson)
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();
                List<String> allBrands = new List<String>();
                foreach (var node in objectJson)
                {
                    allBrands.Add(Convert.ToString(node["BRAND"]));
                }
                // distinct will remove duplicates before we insert them into the db
                IEnumerable<String> brands = allBrands.Distinct<String>();
                foreach (string b in brands)
                {
                    Brand br = new Brand();
                    br.Name = b;
                    _db.Brands.Add(br);
                    _db.SaveChanges();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }

        private bool loadProducts(dynamic objectJson)
        {
            bool loadedItems = false;
            try
            {
                List<Brand>allBrands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();
                foreach (var node in objectJson)
                {
                    Product item = new Product();
                    item.Description = Convert.ToString(node["DESC"].Value);
                    item.ProductName = Convert.ToString(node["PNAME"].Value);
                    item.GraphicName = Convert.ToString(node["GNAME"].Value);
                    item.CostPrice = Convert.ToInt32(node["PRICE"].Value);
                    item.MSRP = Convert.ToInt32(node["MSRP"].Value);
                    item.QtyOnHand = Convert.ToInt32(node["QTYHAND"].Value);
                    item.QtyOnBackOrder = Convert.ToInt32(node["QTYBORDER"].Value);
                    item.Id = Convert.ToString(node["ID"].Value);
                    string brand = Convert.ToString(node["BRAND"].Value);

                    foreach (Brand br in allBrands)
                    {
                        if (br.Name == brand)
                            item.Brand = br;
                    }
                    _db.Products.Add(item);
                    _db.SaveChanges();
                }
                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }
    }
}
