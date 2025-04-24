using System;
using System.Diagnostics.CodeAnalysis;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
   

    [HttpGet]

    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand , string? type)
    {

        return Ok(await repo.GetProductsAsync(brand , type));

    }
    [HttpGet("{id:int}")] //api/products/2

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if(product is null){

            return NotFound(new {message = "Product not found"});
        }
        return product;
    }

    [HttpPost]

    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
       repo.AddProduct(product);

        if(await repo.SaveChangesAsync())
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

        repo.UpdateProduct(product);

        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Updated this Product!");

    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if(product is null) return NotFound();

        repo.DeleteProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleted This Product!");


    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }


    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypeAsync());
    }



    private bool  ProdutExists(int id)
    {
        return repo.ProductExists(id);
    }

    }





