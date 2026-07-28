using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class OrderProduct
    {
        // Composite Key
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation Properties
        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
