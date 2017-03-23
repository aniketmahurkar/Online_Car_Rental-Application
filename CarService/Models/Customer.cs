using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarService.Models
{
    public class Customer
    {
        public int id { get; set; }
        [Required]
        [DisplayName ("PHONE NUMBER")]
        public string phoneNumber { get; set; }
        [EmailAddress]
        [DisplayName("EMAIL ADDRESS")]
        public string email { get; set; }
        [Required]
        [DisplayName("CAR MODEL")]
        public string CarModel { get; set; }
        [Required]
        [DisplayName("CAR MAKE")]
        public string CarMake { get; set; }
        [Required][Range(0, 10)]
        [DisplayName("CAR CAPACITY")]
        public int CarCapacity { get; set; }
        [Required][Range(0,100)]
        [DisplayName("CAR AVERAGE in miles per hour ")]
        public double CarAverage { get; set; }
        [DisplayName("RATE PER HOUR IN DOLLARS")]
        public int PerHourRate { get; set; }
        [DisplayName("FREE FROM (Time)")]
        public int FreeFrom { get; set; }
        [DisplayName("FREE TILL (Time)")]
        public int FreeTo { get; set; }
        public bool Available { get; set; }
        public double longitude { get; set; }
        [Required]
        [DisplayName("STREET 1")]
        public string street1 {get; set; }
        [DisplayName("STREET 2")]
        public string street2 { get; set; }
        [Required]
        [DisplayName("CITY")]
        public string city { get; set; }
        [DisplayName("STATE")]
        public string state { get; set; }
        public double latitude { get; set; }
        public virtual int FrequencyID { get; set; }
        public virtual Frequency frequency { get; set; }

        
    }
}