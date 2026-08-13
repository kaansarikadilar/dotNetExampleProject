using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Stock;
using api_example.Helpers;
using api_example.Models;

namespace api_example.StockRepository
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObjects query);
        Task<Stock?> GetById(int id); // we have a question mark cuz it can be null

        Task<Stock?>GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id,UpdateStockDto stockDto);
        Task<Stock?> DeleteAsync(int id); // we have a question mark cuz it can be null

        Task<bool> StockExists(int id);
    }
}