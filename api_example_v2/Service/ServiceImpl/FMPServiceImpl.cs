using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Stock;
using api_example.Mappers;
using api_example.Models;
using api_example.Service.IService;
using Newtonsoft.Json;
using YahooFinanceApi;

namespace api_example.Service.ServiceImpl
{
    public class FMPServiceImpl : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FMPServiceImpl(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<Stock?> FindStockBySymbol(string symbol)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            // -------------------------------------------------------------
            // Step 1: Try Financial Modeling Prep (FMP) First
            // -------------------------------------------------------------
            try
            {
                var apiKey = _configuration["FMPKey"];
                var symbolUpper = symbol.ToUpper();
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/stable/profile?symbol={symbolUpper}&apikey={apiKey}");  
                
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    
                    if (tasks != null && tasks.Length > 0 && tasks[0] != null)
                    {
                        return tasks[0].ToStockFromFMP();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                string yahooSymbol = symbol.ToUpper();
                if (!yahooSymbol.Contains("."))
                {
                    yahooSymbol = $"{yahooSymbol}.IS";
                }

                var securities = await Yahoo.Symbols(yahooSymbol)
                    .Fields(Field.Symbol, Field.LongName, Field.RegularMarketPrice)
                    .QueryAsync();

                if (securities.ContainsKey(yahooSymbol) && securities[yahooSymbol] != null)
                {
                    var record = securities[yahooSymbol];

                    return new Stock
                    {
                        Symbol = symbol.ToUpper(), // Saves clean symbol like "THYAO" in DB
                        CompanyName = record.LongName ?? symbol.ToUpper(),
                        Purchase = (decimal)record.RegularMarketPrice,
                        LastDiv = 0,
                        Industry = "Borsa Istanbul",
                        MarketCap = 0
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
    }
}