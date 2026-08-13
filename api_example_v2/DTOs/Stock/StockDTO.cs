using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Comment;

namespace api_example.DTOs.Stock
{
    public class StockDTO
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = String.Empty;
        public string CompanyName { get; set; } = String.Empty;
        public decimal Purchase { get; set; } 
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = String.Empty; 
        public long MarketCap { get; set; }
        public List<CommentDTO>? Comments { get; set;}
        
    }
}