using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) 
            :base(P=>
                        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))&&
                        (!specParams.BrandId.HasValue  || P.BrandId== specParams.BrandId.Value)&&
                        (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            if (!string.IsNullOrEmpty(specParams.Sort))
            { 
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        //OrderBy = P => P.Price;
                        OrderBySet(P => P.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc= P => P.Price;
                        OrderByDescSet(P => P.Price);
                        break;
                    default:
                        OrderBySet(P => P.Name);
                        break;
                        
                }
            }else
                OrderBySet(P => P.Name);
            //total 18 - 20
            //index 2
            //size =5
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }

        public ProductWithBrandAndCategorySpecifications(int id):base(P=>P.Id==id)
        {

            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
