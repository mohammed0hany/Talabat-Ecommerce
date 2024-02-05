using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabt.APIs.DTOs
{
    public class OrderDto
    {
        //[Required]
        //public string BuyerEmail { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        
        public AddressDto shipToAddress { get; set; }
    }
}
