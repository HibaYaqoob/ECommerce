using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        // Foreign Key
        public int OrderId { get; set; }

        // Navigation Property
        public Order Order { get; set; }
    }
}
