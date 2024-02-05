using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabt.APIs.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }


        
        public int BrandId { get; set; }
        public virtual string? Brand { get; set; }
        public int CategoryId { get; set; } 
        public virtual string? Category { get; set; }
    }
}
