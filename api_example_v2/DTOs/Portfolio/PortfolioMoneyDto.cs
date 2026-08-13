using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Portfolio
{
    public class PortfolioMoneyDto
    {
        public string AccountOwner { get; set; } = string.Empty;
        public decimal TotalInvested { get; set; }
        public decimal CurrentPortfolioValue { get; set; }
        public decimal TotalProfitLoss { get; set; }
        public string OverallReturn { get; set; } = string.Empty;
        public List<PortfolioDto> Stocks { get; set; } = new();
    }
}