using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Stock
{
    public class FMPStock
    {
         public string symbol { get; set; } = String.Empty;
        public double price { get; set; }
        public double beta { get; set; }
        public int volAvg { get; set; }
        public long marketCap { get; set; }
        public double lastDividend { get; set; }
        public string range { get; set; } = String.Empty;
        public double changes { get; set; }
        public string companyName { get; set; } = String.Empty;
        public string currency { get; set; } = String.Empty;
        public string cik { get; set; } = String.Empty;
        public string isin { get; set; } = String.Empty;
        public string cusip { get; set; } = String.Empty;
        public string exchange { get; set; } = String.Empty;
        public string exchangeShortName { get; set; } = String.Empty;
        public string industry { get; set; } = String.Empty;
        public string website { get; set; } = String.Empty;
        public string description { get; set; } = String.Empty;
        public string ceo { get; set; } = String.Empty;
        public string sector { get; set; } = String.Empty;
        public string country { get; set; } = String.Empty;
        public string fullTimeEmployees { get; set; } = String.Empty;
        public string phone { get; set; } = String.Empty;
        public string address { get; set; } = String.Empty;
        public string city { get; set; } = String.Empty;
        public string state { get; set; } = String.Empty;
        public string zip { get; set; } = String.Empty;
        public double dcfDiff { get; set; }
        public double dcf { get; set; }
        public string image { get; set; } = String.Empty;
        public string ipoDate { get; set; } = String.Empty;
        public bool defaultImage { get; set; }
        public bool isEtf { get; set; }
        public bool isActivelyTrading { get; set; }
        public bool isAdr { get; set; }
        public bool isFund { get; set; }
    }
}