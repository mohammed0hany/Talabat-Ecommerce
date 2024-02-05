using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        //3 signature for 3 methods

        //create order
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);

        //get orders for specific user
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        //get order by id for user
        Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);

        //Get Deliver Methods
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
