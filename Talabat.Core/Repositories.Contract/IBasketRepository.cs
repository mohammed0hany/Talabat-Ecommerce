using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string baskedId); //Get Basket By Id
        Task<CustomerBasket?>UpdateBasketAsync(CustomerBasket basket); //Create Or Update Basket
        Task<bool> DeleteBasketAsync(string baskedId); // Delete Basket
    }
}
