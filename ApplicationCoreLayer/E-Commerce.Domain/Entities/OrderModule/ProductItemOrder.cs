using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public class ProductItemOrder
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string ProductUrl { get; set; } = default!;
    }
}
