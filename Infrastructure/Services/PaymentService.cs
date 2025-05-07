using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    public Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cardId)
    {
        throw new NotImplementedException();
    }
}
