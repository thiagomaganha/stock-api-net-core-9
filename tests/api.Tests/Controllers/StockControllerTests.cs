
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using api.Controllers;
using api.Interfaces;
using api.Dtos;
using System.Text;
using System.IO;
using api.Models;
using api.Helpers;
using api.Mappers;


namespace api.Tests.Controllers
{
    public class StockControllerTests
    {
        
private readonly Mock<IStockService> _serviceMock;
    private readonly StockController _controller;

    public StockControllerTests()
    {
        _serviceMock = new Mock<IStockService>();
        _controller = new StockController(_serviceMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithStockList()
    {
        // Arrange
        var stocks = new List<Stock>
        {
            new Stock { Id = 1, Symbol = "TSLA", CompanyName = "Tesla" },
            new Stock { Id = 2, Symbol = "AAPL", CompanyName = "Apple" }
        };

        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<QueryObject>()))
            .ReturnsAsync(stocks.ToList());

        // Act
        var result = await _controller.Get(new QueryObject());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStocks = Assert.IsType<List<Stock>>(okResult.Value);
        Assert.Equal(2, returnedStocks.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenStockExists()
    {
        var stock = new Stock { Id = 1, Symbol = "StockA", CompanyName = "Company A" };
        _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(stock);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<StockDto>(okResult.Value);
        Assert.Equal("StockA", dto.Symbol);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenStockDoesNotExist()
    {
        _serviceMock.Setup(s => s.GetAsync(99)).ReturnsAsync((Stock)null);

        var result = await _controller.Get(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    // ✅ Test Create
    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var request = new CreateStockRequestDto { Symbol = "TSLA", CompanyName = "Tesla" };
        var stockModel = request.ToStockFromRequestDto();
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Stock>()))
            .Callback<Stock>(s => s.Id = 10)
            .Returns(Task.FromResult(stockModel));

        var result = await _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<StockDto>(createdResult.Value);
        Assert.Equal("TSLA", dto.Symbol);
    }

    [Fact]
    public async Task UploadForm_ShouldReturnCreated_WhenValidImage()
    {
        var imageContent = "fake image content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(imageContent));
        var formFile = new FormFile(stream, 0, stream.Length, "Image", "test.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var request = new CreateStockRequestDto { Symbol = "TSLA", CompanyName = "Tesla", Image = formFile };

        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Stock>()))
            .Callback<Stock>(s => s.Id = 20)
            .Returns(Task.FromResult(new Stock
            {
                Id = 20,
                Symbol = "TSLA",
                CompanyName = "Tesla"
            }));

        var result = await _controller.UploadForm(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<StockDto>(createdResult.Value);
        Assert.Equal("TSLA", dto.Symbol);
    }

    [Fact]
    public async Task UploadForm_ShouldReturnBadRequest_WhenImageMissing()
    {
        var request = new CreateStockRequestDto { Symbol = "Stock", Image = null };

        var result = await _controller.UploadForm(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var problem = Assert.IsType<ProblemDetails>(badRequest.Value);
        Assert.Equal("Image is required.", problem.Title);
    }


    [Fact]
    public async Task Update_ShouldReturnOk_WhenStockExists()
    {
        var updateDto = new UpdateStockRequestDto { Purchase = 100 };
        var updatedStock = new Stock { Id = 1, Purchase = 150 };

        _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<Stock>()))
            .ReturnsAsync(updatedStock);

        var result = await _controller.Update(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<StockDto>(okResult.Value);
        Assert.Equal(150, dto.Purchase);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenStockDoesNotExist()
    {
        var updateDto = new UpdateStockRequestDto { Symbol = "Updtd" };
        _serviceMock.Setup(s => s.UpdateAsync(99, It.IsAny<Stock>()))
            .ReturnsAsync((Stock)null);

        var result = await _controller.Update(99, updateDto);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenStockDeleted()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(new Stock { Id = 1 });

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenStockDoesNotExist()
    {
        _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync((Stock)null);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    }
}