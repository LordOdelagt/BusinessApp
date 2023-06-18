using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGoodsRepository
    {
        string FilePath { get; }

        Goods SearchGoodsByID(int id);
        Goods GetGoodsByName(string? name);
        Goods CreateGoods(string name);
        List<Goods> GetGoods();
        Goods GetFromCsv(string line);
    }
}
