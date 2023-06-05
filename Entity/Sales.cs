using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Sales
    {
        public int SalesID { get; set; }
        public int SalesGoodsID { get; set; }
        public int SalesUnitsID { get; set; }
        public int SalesQuantity { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
