using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace asp.netmvc5.Models
{
    public class Picture
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
 
    public class FileManager
    {
        [Required]
        public int FileId { get; set; }
        [Required]
        public string FileName { get; set; }
        public string Type { get; set; }
        public decimal FileSize { get; set; }
    }
    public class FileManagerDBContext : DbContext
    {
        public DbSet<FileManager> Files { get; set; }
    }
}
