using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;
        public StockController(IStockService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<StockDto>>> Get([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var stocks = await _service.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stocks);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<StockDto>> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _service.GetAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<ActionResult<StockDto>> Create([FromBody] CreateStockRequestDto stock)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = stock.ToStockFromRequestDto();
            await _service.CreateAsync(stockModel);
            return CreatedAtAction(nameof(Get), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<StockDto>> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stock)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _service.UpdateAsync(id, stock.ToStockFromRequestDto());
            if (stockModel == null)
                return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<string>> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await _service.DeleteAsync(id);
            
            if (stock == null)
                return NotFound();

            return NoContent();
        }

    }
}