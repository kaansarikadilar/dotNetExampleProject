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
#pragma warning restore CS8604 // Possible null reference argument.
        return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _appUser.FindByNameAsync(username);
            var stock = await _stockrepository.GetBySymbolAsync(symbol);

            if(stock == null)
            {
                stock = await _fmpService.FindStockBySymbol(symbol);
                if(stock == null)
                {
                    return BadRequest("This stock does not exist");
                }
                else
                {
                    await _stockrepository.CreateAsync(stock);
                }
            }

            if(stock == null)
            {
                return BadRequest("Stock not found :(");
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser!);

            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())){
                return BadRequest("Stock already exists");
            }    
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser!.Id
            };
            await _portfolioRepo.CreateAsync(portfolioModel);

            if(portfolioModel == null)
            {
                return StatusCode(500,"Cant create");
            }
            else
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return Ok("Added : " + portfolioModel.Stock.Symbol);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
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