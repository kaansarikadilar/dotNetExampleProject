using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(50,ErrorMessage ="Symbol cant be longer than 50 characters")]
       public string Symbol { get; set; } = String.Empty;
        [Required]
        [MaxLength(50,ErrorMessage ="CompanyName cant be longer than 50 characters")]
        public string CompanyName { get; set; } = String.Empty;
        [Required]
        [Range(1,100000)]
        public decimal Purchase { get; set; } 
        [Required]
        [Range(0,1000)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Industry cant be longer than 50 characters")]
        public string Industry { get; set; } = String.Empty; 
        [Required]
        [Range(1,100000000000000000)]
        public long MarketCap { get; set; }
    }
}