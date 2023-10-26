using System;
using System.Collections.Generic;

#nullable disable

namespace JWT_ASM3.Models
{
    public partial class Userinfo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
