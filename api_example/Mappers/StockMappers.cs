using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api_example.DTOs.Stock;
using api_example.Models;

namespace api_example.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.toCommentDto()).ToList()
            };
        }
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto StockDTO)
        {
             return new Stock
             {
                 Symbol = StockDTO.Symbol,
                 CompanyName = StockDTO.CompanyName,
                 Purchase = StockDTO.Purchase,
                 LastDiv = StockDTO.LastDiv,
                 Industry = StockDTO.Industry,
                 MarketCap = StockDTO.MarketCap
             };
        }
    }

}