using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderDTO(string BasketId,int DeliveryMethodId,AddressDTO Address);

}
