using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.Models
{
    [Table("Portfolio")]
    public class Portfolio
    {
        public string AppUserId { get; set; } = String.Empty;

        public string UserName => AppUser?.UserName ?? string.Empty;
        public int Quantity { get; set; } = 1;

        public decimal UserBuyPrice { get; set; }
        [NotMapped]
        public decimal LivePrice { get; set; }
        [NotMapped]
        public decimal ProfitLoss => LivePrice - UserBuyPrice;
        [NotMapped]
        public decimal ProfitLossPercentage => UserBuyPrice == 0 
            ? 0 
            : Math.Round(((LivePrice - UserBuyPrice) / UserBuyPrice) * 100, 2);
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    // 🌟 Formatted string (e.g., "13;08;2026") - NOT saved to DB
    [NotMapped]
    public string PurchaseDateFormatted => PurchaseDate.ToString("dd;MM;yyyy");

    // 🌟 Current date formatted without time - NOT saved to DB
    [NotMapped]
    public string CurrentDateFormatted => DateTime.UtcNow.ToString("dd;MM;yyyy");

    public int StockId { get; set; }

    public AppUser? AppUser { get; set; } 
    public Stock? Stock { get; set; }
    }
}