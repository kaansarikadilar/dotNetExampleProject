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

        public string StockId { get; set; } = String.Empty;

        public AppUser? AppUser { get; set; } 

        public Stock? Stock { get; set; }
    }
}