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
        Sales CreateSales(Goods Goods, Units units, Price price, int quantity);
        Sales SearchSalesByID(int id);
        List<Sales> GetSales();
        Sales GetFromCsv(string line);
    }
}
