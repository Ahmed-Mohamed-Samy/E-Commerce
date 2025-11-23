using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public sealed class BasketNotFoundException(string Id) : NotFoundException($"Basket With Id {Id} Not Found")
    {
    }
}
