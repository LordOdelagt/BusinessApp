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
        Price CreatePrice(Category Category, Units units, decimal total);
        Price SearchPriceById(int id);
        Price SearchPriceByMatch(int CategoryId, int unitsId);
        List<Price> GetPrices();
        void Delete(int id);
    }
}
