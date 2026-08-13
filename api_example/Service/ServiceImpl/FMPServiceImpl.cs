using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Stock;
using api_example.Mappers;
using api_example.Models;
using api_example.Service.IService;
using Newtonsoft.Json;

namespace api_example.Service.ServiceImpl
{
    public class FMPServiceImpl : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FMPServiceImpl(HttpClient httpClient,IConfiguration configuration )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<Stock?> FindStockBySymbol(string symbol)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            try
            {
                var apiKey = _configuration["FMPKey"];
                var symbolUpper = symbol.ToUpper();
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/stable/profile?symbol={symbolUpper}&apikey={apiKey}");  
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks?[0];
                    if(stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }
                    return null;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return null;
        }
    }
}