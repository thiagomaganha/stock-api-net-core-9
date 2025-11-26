using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        public StockService(IStockRepository stockRepository)
        {   
            _stockRepository = stockRepository;
        }
        public async Task<Stock> CreateAsync(Stock stock)
        {
            return await _stockRepository.CreateAsync(stock);
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            return await _stockRepository.DeleteAsync(id);
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            return await _stockRepository.GetAllAsync(query);
        }

        public async Task<Stock?> GetAsync(int id)
        {
            return await _stockRepository.GetAsync(id);
        }

        public async Task<Stock> GetBySymbolAsync(string symbol)
        {
            return await _stockRepository.GetBySymbolAsync(symbol);
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _stockRepository.StockExistsAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            return await _stockRepository.UpdateAsync(id, stock);
        }
    }
}