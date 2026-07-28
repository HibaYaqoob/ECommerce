using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; }

        // Navigation Property
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
