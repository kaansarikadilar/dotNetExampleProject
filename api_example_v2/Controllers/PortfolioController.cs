using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api_example.Extensions;
using api_example.Models;
using api_example.Repository;
using api_example.Service.IService;
using api_example.StockRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api_example.Controllers
{
    [Route("/api_example/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _appUser;
        private readonly IStockRepository _stockrepository;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> appUser,IStockRepository stockRepository,IPortfolioRepository portfolioRepo,
        IFMPService fmpService)
        {
            _appUser = appUser;
            _stockrepository = stockRepository;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
                {
                var username =  User.GetUsername();
                var appUser = await _appUser.FindByNameAsync(username);
        #pragma warning disable CS8604 // Possible null reference argument.
                    var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
                return Ok(userPortfolio);
                }
                [HttpGet("getMoney")]
                [Authorize]
                public async Task<IActionResult> GetMoney()
                {
                    var username = User.GetUsername();
                    var appUser = await _appUser.FindByNameAsync(username);

                    if(appUser == null)
                    {
                        return NotFound("User Not Found");
                    }
                    var moneySummary = await _portfolioRepo.GetMoney(appUser);
                    return Ok(moneySummary);
                }
        [HttpPost]
        [Authorize]
      public async Task<IActionResult> AddPortfolio(string symbol, [FromQuery] int quantity = 1)
            {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }
            var username = User.GetUsername();
            var appUser = await _appUser.FindByNameAsync(username);
            if (appUser == null) return NotFound("User not found.");

            var portfolioModel = await _portfolioRepo.CreateAsync(appUser, symbol, quantity);

            if (portfolioModel == null)
            {
                return BadRequest("Stock does not exist.");
            }

            return Ok($"Successfully added {quantity} share(s) of {symbol.ToUpper()} to your portfolio.");
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult>DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _appUser.FindByNameAsync(username);
            if(appUser == null)
            {
                return BadRequest("User cannot be found");
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s=>s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser,symbol);
            }
            else
            {
                return BadRequest("Stock not in portfolio :(");
            }
            return Ok("Deleted : " +symbol.ToUpper());
        }
    }
}