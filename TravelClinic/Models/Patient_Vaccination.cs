using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace asp.netmvc5.Models
{
    public class Patient_Vaccination
    {
            [Key]
            public int AdministeredID { get; set; }
            public Int64 Barcode_NDC { get; set; }
            [ForeignKey("Vaccine")]
            public int VaccineID { get; set; }
            public int Refugee_Num { get; set; }
            public int Patient_Num { get; set; }
            public int Employee_Num { get; set; }
            public decimal Price_Paid { get; set; }
            public string Site_Administered { get; set; }
            public DateTime Date_Administered { get; set; }


            public virtual Vaccine Vaccine { get; set; }
            //public virtual ApplicationUser ApplicationUser { get; set; }


    }
}