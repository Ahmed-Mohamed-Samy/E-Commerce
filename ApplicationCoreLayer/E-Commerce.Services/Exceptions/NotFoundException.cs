using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundException(string Msg) : Exception(Msg)
    {
    }
}
