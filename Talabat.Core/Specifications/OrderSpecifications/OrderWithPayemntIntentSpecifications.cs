using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderWithPayemntIntentSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPayemntIntentSpecifications(string paymentIntendId) : base(O => O.PaymentIntentId == paymentIntendId)
        {
            Includes.Add(O => O.Items);
        }
    }
}
