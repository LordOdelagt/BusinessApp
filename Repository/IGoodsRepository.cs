﻿using Entity;
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
        Goods CreateGoods(string name, int categoryId, int unitsId, int quantity);
        List<Goods> GetGoods();
        void Delete(int id);
    }
}
