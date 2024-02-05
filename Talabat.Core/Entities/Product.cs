using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }


        //ProductBrandId
        public int BrandId { get; set; } //fk column => ProductBrand
        public virtual ProductBrand Brand { get; set; } //Navigational prop one
        public int CategoryId { get; set; } //fk column => ProductCategory
        public virtual ProductCategory Category { get; set; } //Navigational prop one
    }
}
