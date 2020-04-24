using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace User.Models
{
    public class UserItem
    {
        // public long Id { get; set; }
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        // -1 means deleted; 0 means normal
    }
}