using EC_Cart_Service.Models;
using EC_Cart_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EC_Cart_Service.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly ICartStore _cartStore;

    public CartsController(ICartStore cartStore)
    {
        _cartStore = cartStore;
    }

    [HttpGet("{userId:int}")]
    public ActionResult<Cart> GetCart(int userId)
    {
        return Ok(_cartStore.GetOrCreate(userId));
    }

    [HttpPost("{userId:int}/items")]
    public ActionResult<Cart> AddItem(int userId, [FromBody] UpsertCartItemRequest request)
    {
        if (request.ProduitId <= 0)
        {
            return BadRequest(new { error = "ProduitId doit etre superieur a 0." });
        }

        if (request.Quantite <= 0)
        {
            return BadRequest(new { error = "La quantite doit etre superieure a 0." });
        }

        var cart = _cartStore.AddItem(userId, request);
        return Ok(cart);
    }

    [HttpPut("{userId:int}/items/{productId:int}")]
    public ActionResult<Cart> UpdateItemQuantity(int userId, int productId, [FromBody] UpsertCartItemRequest request)
    {
        if (request.Quantite <= 0)
        {
            return BadRequest(new { error = "La quantite doit etre superieure a 0." });
        }

        var cart = _cartStore.UpdateItemQuantity(userId, productId, request.Quantite);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpDelete("{userId:int}/items/{productId:int}")]
    public IActionResult RemoveItem(int userId, int productId)
    {
        return _cartStore.RemoveItem(userId, productId) ? NoContent() : NotFound();
    }

    [HttpDelete("{userId:int}")]
    public IActionResult ClearCart(int userId)
    {
        return _cartStore.Clear(userId) ? NoContent() : NotFound();
    }
}
