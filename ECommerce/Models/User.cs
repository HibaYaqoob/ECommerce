using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        // Navigation Property
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
