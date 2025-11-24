using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioService _portfolioService;
        private readonly IStockService _stockService;

        public PortfolioController(UserManager<AppUser> userManager, 
                                    IPortfolioService portfolioService, 
                                    IStockService stockService)
        {
            _userManager = userManager;
            _portfolioService = portfolioService;
            _stockService = stockService;
        }
    
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Unauthorized();

            var userPortfolio = await _portfolioService.GetUserPortfolio(user);

            if (userPortfolio == null)
                return NotFound();

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
                return Unauthorized();
            
            var stock = await _stockService.GetBySymbolAsync(symbol);
            if (stock == null)
                return BadRequest("Stock does not exist");

            var userPortfolio = await _portfolioService.GetUserPortfolio(user);
            if (userPortfolio.Any(x => x.Symbol == symbol))
                return BadRequest("Stock already in portfolio");    

            var portfolio = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id
            };
            
            var result = await _portfolioService.AddAsync(portfolio);

            if(result == null)  
            return StatusCode(500, "Failed to add stock to portfolio");
            
            return Created("portfolio", result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized();

            var userPortfolio = await _portfolioService.GetUserPortfolio(user);
            if (!userPortfolio.Any(x => x.Symbol == symbol))
                return BadRequest("Stock does not exist in portfolio");

            var result = await _portfolioService.DeleteAsync(user, symbol);
            if (result == null)
                return StatusCode(500, "Failed to delete stock from portfolio");

            return NoContent();
        }
    }
}