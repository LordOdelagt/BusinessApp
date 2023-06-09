﻿using Entity;
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
        Sales CreateSales(int goodsId, int categoryId, int unitsId, decimal price, int quantity);
        Sales SearchSalesByID(int id);
        List<Sales> GetSales();
        void Delete(int id);
    }
}
