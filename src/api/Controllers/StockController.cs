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
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; 


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

            return Ok(stockDto);
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

        [HttpPost]
        [Route("form")]
        public async Task<ActionResult<StockDto>> UploadForm([FromForm] CreateStockRequestDto stock)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (stock.Image is null || stock.Image.Length == 0)
            return BadRequest(new ProblemDetails { Title = "Image is required." });

            if (stock.Image.Length > MaxFileSizeBytes)
            return BadRequest(new ProblemDetails { Title = $"Image exceeds max size of {MaxFileSizeBytes / (1024 * 1024)} MB." });
            
            var ext = Path.GetExtension(stock.Image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                return BadRequest(new ProblemDetails { Title = "Unsupported image extension." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = "{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await stock.Image.CopyToAsync(stream);
            }

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