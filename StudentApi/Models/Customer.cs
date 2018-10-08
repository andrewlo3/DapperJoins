﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.Models
{
    public class Customer
    {
        public int CustomerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
