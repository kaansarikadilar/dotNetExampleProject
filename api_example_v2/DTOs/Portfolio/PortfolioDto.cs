using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Portfolio
{
    public class PortfolioDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public string CompanyName { get; set; } = string.Empty;
        public decimal UserBuyPrice { get; set; }
        public decimal LivePrice { get; set; }
        
        // Calculated Profit & Loss properties
        public decimal ProfitLoss => LivePrice - UserBuyPrice;
        public string ProfitLossPercentage => UserBuyPrice == 0 
            ? "0%" 
            : $"{Math.Round(((LivePrice - UserBuyPrice) / UserBuyPrice) * 100, 2)}%";

        public string PurchaseDate { get; set; } = string.Empty;
        public string CurrentDate { get; set; } = string.Empty;
    }
}