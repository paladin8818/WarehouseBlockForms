using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarehouseBlockForms.Models
{
    public interface IRowChangeable
    {
        bool up();
        bool down();
    }
}
