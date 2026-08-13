using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Data;
using api_example.DTOs.Stock;
using api_example.Helpers;
using api_example.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_example.StockRepository.StockRepositoryImpl
{
    public class StockRepositoryImpl : IStockRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public StockRepositoryImpl(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<List<Stock>> GetAllAsync(QueryObjects query)
        {
        var stocks = _applicationDBContext.Stock
        .Include(c => c.Comments)
        .ThenInclude(a => a.AppUser)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }
        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
            else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
            }
            else
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Id) : stocks.OrderBy(s => s.Id);
            }
        }
        else
        {
                stocks = stocks.OrderBy(s => s.Id);
        }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _applicationDBContext.Stock.AddAsync(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel =
            await _applicationDBContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
             if(stockModel == null)
            {
                return null;
            }
            _applicationDBContext.Stock.Remove(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return stockModel;
        }
        
        public async Task<Stock?> GetById(int id)
        {
                return await _applicationDBContext.Stock.Include(c => c.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync (i => i.Id == id);
        }
        public async Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto)
        {
            var existingStock = 
            await _applicationDBContext.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(existingStock == null)
            {
                return null;
            }
             existingStock.Symbol = stockDto.Symbol;
             existingStock.CompanyName = stockDto.CompanyName;
             existingStock.Purchase = stockDto.Purchase;
             existingStock.LastDiv = stockDto.LastDiv;
             existingStock.Industry = stockDto.Industry;
             existingStock.MarketCap = stockDto.MarketCap;

             await _applicationDBContext.SaveChangesAsync();
             return existingStock;
        }
        public Task<bool> StockExists(int id)
        {
            return  _applicationDBContext.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _applicationDBContext.Stock.FirstOrDefaultAsync(s=>s.Symbol == symbol);
        }
    }
}