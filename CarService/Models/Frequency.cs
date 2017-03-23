using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarService.Models
{
    public class Frequency
    {
        public int id { get; set; }
        public string description { get; set; }
        
        public virtual List<Customer> customer { get; set; }
    }
}