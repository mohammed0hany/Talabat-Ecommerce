using Talabat.Core.Entities.Order_Aggregate;

namespace Talabt.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public  string DeliveryMethod { get; set; }
        public  decimal DeliveryMethodCost { get; set; }
        public  ICollection<OrderItemDto> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
