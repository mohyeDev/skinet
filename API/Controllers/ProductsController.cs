using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unit) : BaseApiController
{
   

    [HttpGet]

    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProcutSpecParams specParams)
    {
        var spec =new ProductSpecification(specParams);


        return await CreatePagedResult(unit.Repository<Product>(),spec,specParams.PageIndex,specParams.PageSize);
    }
    [HttpGet("{id:int}")] //api/products/2

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
        if(product is null){

            return NotFound(new {message = "Product not found"});
        }
        return product;
    }

    [HttpPost]

    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
       unit.Repository<Product>().Add(product);

        if(await unit.Complete())
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

        unit.Repository<Product>().Update(product);

        if(await unit.Complete() )
        {
            return NoContent();
        }

        return BadRequest("Problem Updated this Product!");

    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
        if(product is null) return NotFound();

        unit.Repository<Product>().Remove(product);
        if(await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleted This Product!");


    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {

        var spec = new BrandListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }


    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {

        var spec = new TypeListSpecification();
        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }



    private bool  ProdutExists(int id)
    {
        return unit.Repository<Product>().Exists(id);
    }

    }





