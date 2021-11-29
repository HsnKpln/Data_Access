using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Models
{
    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
