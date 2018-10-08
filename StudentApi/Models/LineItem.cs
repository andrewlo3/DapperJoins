using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.Models
{
    public class LineItem
    {
        // public int InvoiceNumber { get; set; }
        public string ProductCode { get; set; }
        public int LineUnits { get; set; }
        public decimal LinePrice { get; set; }
    }
}
