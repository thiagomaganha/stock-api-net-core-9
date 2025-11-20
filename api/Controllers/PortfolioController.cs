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
        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IStockRepository _stockRepository;

        public PortfolioController(UserManager<AppUser> userManager, 
                                    IPortfolioRepository portfolioRepository, 
                                    IStockRepository stockRepository)
        {
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
        }
    
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Unauthorized();

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);

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
            
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
                return BadRequest("Stock does not exist");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            if (userPortfolio.Any(x => x.Symbol == symbol))
                return BadRequest("Stock already in portfolio");    

            var portfolio = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id
            };
            
            await _portfolioRepository.AddAsync(portfolio);
            
            return Ok();
        }
    
    }
}