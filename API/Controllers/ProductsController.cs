using Core.Entities;
using Core.Specification;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
   

    [HttpGet]

    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand , string? type , string? sort)
    {
        var spec =new ProductSpecification(brand , type,sort);

        var products = await repo.ListAsync(spec);

        return Ok(products);
    }
    [HttpGet("{id:int}")] //api/products/2

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if(product is null){

            return NotFound(new {message = "Product not found"});
        }
        return product;
    }

    [HttpPost]

    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
       repo.Add(product);

        if(await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct",new {id = product.Id} , product);
        }
        return BadRequest("Problem Creating Product");
    }

    [HttpPut("{id:int}")]

    public async Task<ActionResult> UpdateProduct(int id , Product product)
    {

        if(product.Id != id || !ProdutExists(id))
        {
            return BadRequest("Cannot Update This Product");
        }

        repo.Update(product);

        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Updated this Product!");

    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if(product is null) return NotFound();

        repo.Remove(product);
        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleted This Product!");


    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {

        return Ok();
    }


    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok();
    }



    private bool  ProdutExists(int id)
    {
        return repo.Exists(id);
    }

    }





