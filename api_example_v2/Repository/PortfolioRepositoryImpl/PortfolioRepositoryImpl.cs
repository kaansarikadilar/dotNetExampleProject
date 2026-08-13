using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using api_example.Data;
using api_example.DTOs.Portfolio;
using api_example.Models;
using api_example.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_example.Repository.PortfolioRepositoryImpl
{
    public class PortfolioRepositoryImpl : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IFMPService _fmpService;
        public PortfolioRepositoryImpl(ApplicationDBContext context,IFMPService fMPService)
        {
            _context = context;
            _fmpService = fMPService;
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<Portfolio?> CreateAsync(AppUser appUser, string symbol, int quantity)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            var stock = await _context.Stock
                .FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());

            if (stock == null)
            {
                var fmpStock = await _fmpService.FindStockBySymbol(symbol);
                if (fmpStock == null)
                {
                    return null; // Stock does not exist on FMP
                }

                stock = fmpStock;
                await _context.Stock.AddAsync(stock);
                await _context.SaveChangesAsync();
            }

            // 3. Check if user already owns this stock in their portfolio
            var existingPortfolio = await _context.Portfolios
                .FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.StockId == stock.Id);

            if (existingPortfolio != null)
            {
                // Stock exists in user's portfolio: Update Quantity
                existingPortfolio.Quantity += quantity;
                await _context.SaveChangesAsync();
                return existingPortfolio;
            }

            // 4. Stock does not exist in user's portfolio: Create new Portfolio entry
            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
                UserBuyPrice = stock.Purchase,
                Quantity = quantity,
                PurchaseDate = DateTime.UtcNow
            };

            await _context.Portfolios.AddAsync(portfolioModel);
            await _context.SaveChangesAsync();

            return portfolioModel;
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
// 1. Added 'async' keyword here 👇
        public async Task<PortfolioMoneyDto> GetMoney(AppUser appUser)
        {
            var userPortfolios = await _context.Portfolios
                .Where(u => u.AppUserId == appUser.Id)
                .Include(p => p.Stock)
                .Include(p => p.AppUser)
                .ToListAsync();

            var stockDtos = new List<PortfolioDto>();

            foreach (var item in userPortfolios)
            {
                // 3. Added 'await' keyword here 👇
                var fmpStock = await _fmpService.FindStockBySymbol(item.Stock!.Symbol);
                
                decimal livePrice = fmpStock != null ? (decimal)fmpStock.Purchase : item.UserBuyPrice;

                stockDtos.Add(new PortfolioDto
                {
                    UserName = item.UserName,
                    Symbol = item.Stock.Symbol,
                    CompanyName = item.Stock.CompanyName,
                    UserBuyPrice = item.Stock.Purchase,
                    LivePrice = livePrice,
                    Quantity = item.Quantity, 
                    PurchaseDate = item.PurchaseDateFormatted,
                    CurrentDate = item.CurrentDateFormatted
                });
            }
            decimal totalInvested = stockDtos.Sum(s => s.UserBuyPrice * s.Quantity);
            decimal totalValue = stockDtos.Sum(s => s.LivePrice * s.Quantity);
            decimal totalProfitLoss = totalValue - totalInvested;

            return new PortfolioMoneyDto
            {
                AccountOwner = appUser.UserName ?? string.Empty,
                TotalInvested = totalInvested,
                CurrentPortfolioValue = totalValue,
                TotalProfitLoss = totalProfitLoss,
                OverallReturn = totalInvested == 0 
                    ? "0%" 
                    : $"{Math.Round((totalProfitLoss / totalInvested) * 100, 2)}%",
                Stocks = stockDtos
            };
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