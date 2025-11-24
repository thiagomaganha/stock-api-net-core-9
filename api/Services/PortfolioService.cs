using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public async Task<Portfolio> AddAsync(Portfolio portfolio)
        {
            return await _portfolioRepository.AddAsync(portfolio);
        }

        public async Task<Portfolio> DeleteAsync(AppUser user, string symbol)
        {
            return await _portfolioRepository.DeleteAsync(user, symbol);
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _portfolioRepository.GetUserPortfolio(user);
        }
    }
}