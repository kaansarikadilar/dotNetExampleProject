using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.Helpers
{
    public class CommentQueryObject
    {
        public string Symbol { get; set; } = String.Empty;
        public bool IsDescending { get; set; } = true;
    }
}