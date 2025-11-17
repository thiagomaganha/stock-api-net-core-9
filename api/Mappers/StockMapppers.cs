using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class StockMapppers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Dividend = stockModel.Dividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase,
                Comments = stockModel.Comments != null ? [.. stockModel.Comments.Select(x => x.ToCommentDto())] : []
            };
        }

        public static Stock ToStockFromRequestDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Dividend = stockDto.Dividend,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap,
                Purchase = stockDto.Purchase
            };
        }

        public static Stock ToStockFromRequestDto(this UpdateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Dividend = stockDto.Dividend,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap,
                Purchase = stockDto.Purchase
            };
        }
        
    }
}