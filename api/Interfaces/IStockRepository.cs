using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock> GetBySymbolAsync(string symbol);
        Task<Stock?> UpdateAsync(int id, Stock stock);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExistsAsync(int id);
    }
}