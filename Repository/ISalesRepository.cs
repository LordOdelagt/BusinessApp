using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISalesRepository
    {
        string FilePath { get; }
        Sales CreateSales(int goodsId, int unitsId, int quantity);
    }
}
