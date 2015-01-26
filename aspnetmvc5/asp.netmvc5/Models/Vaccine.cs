using System;
using System.Data.Entity;

namespace asp.netmvc5.Models
{
    public class Vaccine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Vendor_ { get; set; }
        public decimal Dosage { get; set; }
        public DateTime Date_Added { get; set; }
        public DateTime Date_Expire { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }



    }
    public class VaccineDBContext : DbContext
    {
        public DbSet<Vaccine> Vaccines { get; set; }
    }
}