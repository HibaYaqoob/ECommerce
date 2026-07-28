using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }

        // Navigation Property
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
