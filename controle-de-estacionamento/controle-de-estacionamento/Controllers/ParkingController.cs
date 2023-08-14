using controle_de_estacionamento.Services.IServices;
using Infrastructure.Models;
using Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace controle_de_estacionamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
    private readonly IParkingService parkingService;
    public ParkingController(IParkingService parkingService)
    {
        this.parkingService = parkingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Parking>>> GetAllAsync()
    {
        return Ok(await this.parkingService.GetAllAsync());
    }

    [HttpGet("getById")]
    public async Task<ActionResult<Parking>> GetByIdAsync(long id)
    {
        return Ok(await this.parkingService.GetByIdAsync(id));
    }

    [HttpPost("CheckIn")]
    public async Task<ActionResult> CheckIn(CheckInDTO entity)
    {
        return Ok(await parkingService.CheckIn(entity));
    }

    [HttpPut("CheckOut")]
    public async Task<ActionResult> CheckOut(CheckOutDTO entity)
    {
        return Ok(await parkingService.CheckOut(entity));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        return Ok(await parkingService.DeleteAsync(id));
    }
}
