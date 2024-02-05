using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpeccifications;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service
{
   
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenaricRepository<Product> _productRepository;
        //private readonly IGenaricRepository<DeliveryMethod> _deliveryRepository;
        //private readonly IGenaricRepository<Order> _orderRepository;

        public OrderService(IBasketRepository basketRepository ,IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
           _basketRepository = basketRepository;
           _unitOfWork = unitOfWork;
           _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //Get Basket from basket repo
            var basket =await  _basketRepository.GetBasketAsync(basketId);
            //Get selected itens at basket from product repo and create order items
            var orderItems= new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product =await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrderd = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrderd, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            //calculate subtotal
            var subtotal = orderItems.Sum(orderItem=>orderItem.Price * orderItem.Quantity);

            //get delivey method
            var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            var ordersRepo =_unitOfWork.Repository<Order>();
            var orderSpecs = new OrderWithPayemntIntentSpecifications(basket.PayemntIntentId);
            var existingOrder = await ordersRepo.GetWithSpecificationAsync(orderSpecs);
            if(existingOrder != null)
            {
                ordersRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePayementIntent(basketId);
            }
            //create order
            var order = new Order(buyerEmail,shippingAddress,deliveryMethod,orderItems,subtotal,basket.PayemntIntentId);
            await ordersRepo.AddAsync(order);

            //save changes
            var result =await _unitOfWork.CompleteAsync();
            if(result <= 0)
                return null;
            return order;



        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods=await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpec(orderId,buyerEmail);
            var order = _unitOfWork.Repository<Order>().GetWithSpecificationAsync(spec);
            return (order);
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpec(buyerEmail);
            var orders = _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(spec);
            return orders;
        }
    }
}
