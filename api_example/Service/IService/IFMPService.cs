using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.Models;

namespace api_example.Service.IService
{
    public interface IFMPService
    {
        Task<Stock> FindStockBySymbol(string symbol);
    }
}