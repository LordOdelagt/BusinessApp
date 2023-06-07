﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entity
{
    public class Sales
    {
        public int SalesID { get; set; }
        public int SalesGoodsID { get; set; }
        public int SalesUnitsID { get; set; }
        public int SalesQuantity { get; set; }
        public decimal SalesPrice { get; set; }

        public void Deconstruct(out int ID, out int GoodsID, out int UnitsID, out int Quantity, out decimal Price)
        {
            ID = this.SalesID;
            GoodsID = this.SalesGoodsID;
            UnitsID = this.SalesUnitsID;
            Quantity = this.SalesQuantity;
            Price = this.SalesPrice;
        }

    }
}
