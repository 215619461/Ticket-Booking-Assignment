using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Orders.Models
{
    public class OrdersItem
    {
        // public long ID { get; set; }
        [Key]
        public long TicketId { get; set; }
        public int Fare { get; set; }
        public long UserId { get; set; }
        public int Status { get; set; }
        // -1 means deleted; 0 means normal; 1 means wait for pay; 2 means already paid
    }
}
