using API.Dtos;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrderController (ICartService cartService , IUnitOfWork unit): BaseApiController
{

    [HttpPost]

    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto){

        var email = User.GetEmail();

        var cart = await cartService.GetCartAsync(orderDto.CartId );

        if(cart is null) return BadRequest("Cart Not Found!");

        if(cart.PaymentIntentId is null) return BadRequest("No Payment Intent for this order!");

        var items = new List<OrderItem>();

        foreach(var item in cart.Items){

            var productItem = await unit.Repository<Product>().GetByIdAsync(item.ProductId) ; 

            if(productItem is null) return BadRequest("Problem with the order!");

            var itemOrdered = new ProductItemOrdered{
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl ,

            };

            var orderItem = new OrderItem{
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);


        }


        var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

        if(deliveryMethod is null ) return BadRequest("No Delivery Method Selected!");

        var order =new Order{
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDto.shippingAddress,
            Subtotal = items.Sum(x => x.Price * x.Quantity),
            PaymentSummary = orderDto.paymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email

        };  

        unit.Repository<Order>().Add(order); 

        if(await unit.Complete()){
            return order;
        }

        return BadRequest("Problem Creating Order!");

    }

}
