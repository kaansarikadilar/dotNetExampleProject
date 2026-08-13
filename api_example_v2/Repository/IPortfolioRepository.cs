using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Portfolio;
using api_example.Models;

namespace api_example.Repository
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<PortfolioMoneyDto> GetMoney(AppUser appUser); // 👈 New method
        Task<Portfolio> CreateAsync(AppUser appUser, string symbol, int quantity);
        Task<Portfolio?>DeletePortfolio(AppUser appUser,string symbol);
    }
}