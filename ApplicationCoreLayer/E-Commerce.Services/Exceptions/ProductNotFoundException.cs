using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public sealed class ProductNotFoundException(int Id) : NotFoundException($"Product with Id {Id} Not Found")
    {
    }
}
