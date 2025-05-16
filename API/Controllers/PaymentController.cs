using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;


namespace API.Controllers;

public class PaymentController(IPaymentService paymentService
, IUnitOfWork unit, ILogger<PaymentController> logger ,IConfiguration config) : BaseApiController
{

    private readonly string _whSecret = config["StripeSettings:WhSecret"]!;





    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
    {
        var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);

        if (cart is null) return BadRequest("Problem With Your Cart");

        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {

        return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync());

    }

[HttpPost("webhook")]
public async Task<IActionResult> StripeWebHook()
{
    var json = await new StreamReader(Request.Body).ReadToEndAsync();
    logger.LogInformation("Stripe Webhook Received JSON: {Json}", json);

    try
    {
        var signatureHeader = Request.Headers["Stripe-Signature"];
        logger.LogInformation("Stripe Signature Header: {Signature}", signatureHeader.ToString());

        if (string.IsNullOrWhiteSpace(signatureHeader))
        {
            logger.LogError("Stripe-Signature header is missing");
            return BadRequest("Missing signature");
        }

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _whSecret,  throwOnApiVersionMismatch: false);
            logger.LogInformation("Stripe Event Constructed: {Type}", stripeEvent.Type);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error constructing Stripe event");
            return BadRequest("Invalid event signature");
        }

        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;

            if (intent == null)
            {
                logger.LogError("Stripe PaymentIntent is null");
                return BadRequest("Invalid PaymentIntent");
            }

            logger.LogInformation("PaymentIntent ID: {Id}, Amount: {Amount}, Status: {Status}", 
                intent.Id, intent.Amount, intent.Status);

            try
            {
                await handlePaymentIntentSucceeded(intent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in handlePaymentIntentSucceeded");
                return StatusCode(500, "Error while handling intent");
            }
        }

        return Ok();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled error in Stripe webhook");
        return StatusCode(500, "General webhook error");
    }
}


private async Task handlePaymentIntentSucceeded(PaymentIntent intent)
{
    try
    {
        logger.LogInformation("▶ Handling PaymentIntent: {Id}", intent.Id);

        var spec = new OrderSpecification(intent.Id, true);
        var order = await unit.Repository<Core.Entities.OrderAggregate.Order>().GetEntityWithSpec(spec);

        if (order == null)
        {
            logger.LogWarning("⚠ Order not found for PaymentIntent ID: {IntentId}", intent.Id);
            return; // Don’t throw — let it return 200 to Stripe
        }

        var totalExpected = (long)order.GetTotal() * 100;
        logger.LogInformation("💰 Order total: {Expected} vs Stripe Amount: {Actual}", totalExpected, intent.Amount);

        if (totalExpected != intent.Amount)
        {
            order.Status = OrderStatus.PaymentMisMatch;
        }
        else
        {
            order.Status = OrderStatus.PaymentRecived;
        }

        await unit.Complete();
        logger.LogInformation("✅ Order updated: {OrderId} with status {Status}", order.Id, order.Status);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Exception inside handlePaymentIntentSucceeded");
        throw; // Let the controller return 500 for now to see full stack trace
    }
}


    private Event ConstructStripeEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);

        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Faild To Construct Stripe Event!");
            throw new StripeException("Invalid Signatrue!");
        }
    }





}
