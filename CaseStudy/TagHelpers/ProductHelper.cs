using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using System.Text;
using CaseStudy.Models;
using Newtonsoft.Json;
using CaseStudy.Utils;

namespace CaseStudy.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("products", Attributes = BrandIdAttribute)]
    public class ProductHelper : TagHelper
    {
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private const string BrandIdAttribute = "brand";
        [HtmlAttributeName(BrandIdAttribute)]
        public string BrandId { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_session.Get<ProductViewModel[]>(SessionVariables.Product) != null && Convert.ToInt32(BrandId) > 0)
            {
                var innerHtml = new StringBuilder();
                ProductViewModel[] prd = _session.Get<ProductViewModel[]>(SessionVariables.Product);
                innerHtml.Append("<h5>Products</h5>");
                innerHtml.Append("<div class=\"row w-100 m-1\" style=\"overflow-y:scroll;height:60vh;\">");
                foreach (ProductViewModel item in prd)
                {
                    if (item.BrandID == Convert.ToInt32(BrandId))
                    {
                        // remove double apostrophe
                        item.Description = item.Description.Contains("'") ?
                        item.Description.Replace("'", "") : item.Description;
                        var itemjson = JsonConvert.SerializeObject(item);
                        var btn = "brbtn" + item.Id;
                        innerHtml.Append("<div id=\"item\" class=\"col-sm-3 col-xs-12 text-center\">");
                        innerHtml.Append("<img class=\"img-thumbnail\" src=\"/images/ShoesCollection/" + item.GRPName + "\"/><br />");
                        if (item.Description.Length > 25)
                        {
                            innerHtml.Append("<span class=\"m-0\" style=\"font-size:large;font-weight:bold;\">" + item.PRDName + "</span><br />");
                            innerHtml.Append("<span class=\"m-0\" style=\"font-size:large;font-weight:normal;\">" + String.Format("{0:C}", item.MSRP) + "</span><br />");
                        }
                        else
                        {
                            innerHtml.Append("<span style=\"font-size:large;font-weight:bold;\">" + item.Description + "</span>");
                        }
                        innerHtml.Append("<a id=\"" + btn + "\" href=\"#details_popup\" data-toggle=\"modal\" class=\"btn btn-outline-dark pt-2 pb-2\" data-id=" + item.Id + " data-details='" + itemjson + "'>Details</a></div>");
                    }
                }
                output.Content.SetHtmlContent(innerHtml.ToString());
            }
        }
    }
 }

