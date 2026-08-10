using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api_example.Data;
using api_example.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_example.Repository.PortfolioRepositoryImpl
{
    public class PortfolioRepositoryImpl : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepositoryImpl(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
                await _context.Portfolios.AddAsync(portfolio);
                await _context.SaveChangesAsync();
                 return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x=>x.AppUserId == appUser.Id && x.Stock!.Symbol.ToLower() == symbol.ToLower());
            if(portfolioModel == null)
            {
                return null;
            }
            _context.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }


        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _context.Portfolios.Where(u=>u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
            }).ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}