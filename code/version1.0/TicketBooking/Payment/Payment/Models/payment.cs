using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Payment.Models
{
    public class payment
    {
        // public long ID { get; set; }
        [Key]
        public long UserID { get; set; }
        public int Balance { get; set; }
    }
}