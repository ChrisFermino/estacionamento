using controle_de_estacionamento.Services.IServices;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace controle_de_estacionamento.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    private readonly IPriceService priceService;

    public PriceController(IPriceService priceService)
    {
        this.priceService = priceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Price>>> GetAsync()
    {
        return Ok(await priceService.GetAllAsync());
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<List<Price>>> GetByIdAsync(long id = 0)
    {
        return Ok(await priceService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Price entity)
    {
        if (entity.EndDate < entity.StartDate)
        {
            return BadRequest("The output date must be greater than the input date!");
        }
        return Ok(await priceService.CreateAsync(entity));
    }

    [HttpPut]
    public async Task<ActionResult> EditAsync(Price entity)
    {
        return Ok(await priceService.EditAsync(entity));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        return Ok(await priceService.DeleteAsync(id));
    }
}
