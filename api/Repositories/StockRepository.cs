using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
                return null;

            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(x => x.Comments).ThenInclude(x => x.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Industry))
            {
                stocks = stocks.Where(x => x.Industry.Contains(query.Industry));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.InvariantCultureIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.Symbol) : stocks.OrderBy(x => x.Symbol);
                }
                if (query.SortBy.Equals("CompanyName", StringComparison.InvariantCultureIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.CompanyName) : stocks.OrderBy(x => x.CompanyName);
                }
                if (query.SortBy.Equals("Industry", StringComparison.InvariantCultureIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.Industry) : stocks.OrderBy(x => x.Industry);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize);
            
            return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetAsync(int id)
        {
            return await _context.Stock.Include(x => x.Comments).FirstAsync(x => x.Id == id) ?? await _context.Stock.FindAsync(id);
        }

        public async Task<Stock> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(x => x.Symbol == symbol);
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
                return null;

            stockModel.Symbol = stock.Symbol;   
            stockModel.CompanyName = stock.CompanyName;
            stockModel.Dividend = stock.Dividend;
            stockModel.Industry = stock.Industry;
            stockModel.MarketCap = stock.MarketCap;
            stockModel.Purchase = stock.Purchase;

            await _context.SaveChangesAsync();
            return stock;
        }
    }
}