using FinShark.Dtos.Stock;
using FinShark.Helpers;
using FinShark.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepo;
    public StockController(IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var stocks = await _stockRepo.GetAllAsync(query);
        var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

		return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var stock = await _stockRepo.GetByIdAsync(id);
        if (stock == null)
        {
			return NotFound();
		} 
		return Ok(stock.ToStockDto());
	}

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stock)
    {
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var stockModel = stock.FromCreateStockDto();
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(Get), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto model)
    {
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var stock = await _stockRepo.UpdateAsync(id, model);
        if(stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var stockModel = await _stockRepo.DeleteAsync(id);
        if(stockModel == null)
        {
            return NotFound();
        }
    
        return NoContent();
    }
}