using API.RequestHelper;
using Core.Entities;
using Core.Specification;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
   

    [HttpGet]

    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProcutSpecParams specParams)
    {
        var spec =new ProductSpecification(specParams);


        return await CreatePagedResult(repo,spec,specParams.PageIndex,specParams.PageSize);
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

        var spec = new BrandListSpecification();

        return Ok(await repo.ListAsync(spec));
    }


    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {

        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }



    private bool  ProdutExists(int id)
    {
        return repo.Exists(id);
    }

    }





