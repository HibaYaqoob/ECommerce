using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Property
        public User User { get; set; }

        // Navigation Property
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        // One-to-One Navigation
        public Review Review { get; set; }
    }
}
