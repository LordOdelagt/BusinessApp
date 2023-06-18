using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPriceRepository
    {
        string FilePath { get; }
        Price CreatePrice(Goods Goods, Units units, decimal total);
        Price SearchPriceById(int id);
        Price SearchPriceByMatch(Goods Goods, Units units);
        List<Price> GetPrices();
        Price GetFromCsv(string line);
    }
}
