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
        Price CreatePrice(Goods goods, Units units, decimal total);
        Price SearchPriceById(int id);
        Price SearchPriceByMatch(Goods goods, Units units);
        List<Price> GetPrices();
        Price GetFromString(string line);
    }
}
