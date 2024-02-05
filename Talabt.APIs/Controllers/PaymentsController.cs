using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Talabt.APIs.Errors;

namespace Talabt.APIs.Controllers
{

    public class PaymentsController : BaseAPIController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_42e84a00e91600c431391bc96b1d49bebb38f918ddfe8e17f83da9a844fa3e64";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")] //Post: api/Payments/baskedId
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePayementIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400,"An Error With Your Basket"));
            return Ok(basket);
        }


        [HttpPost("webhook")] // POST:/api/Payments/webhook
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var paymentIntent =(PaymentIntent) stripeEvent.Data.Object;

                Order order;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order =await _paymentService.UpdatePayemntIntentToSucceededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment Is Succeeded",paymentIntent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order =await _paymentService.UpdatePayemntIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment Is Failed",paymentIntent.Id);
                        break;
                }

                return Ok();

        }
    }
}
