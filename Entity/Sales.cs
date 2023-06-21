using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entity
{
    public class Sales : Common
    {
        public int SalesId { get; set; }
        public int SalesCategoryId { get; set; }
        public int SalesUnitsId { get; set; }
        public int SalesQuantity { get; set; }
        public decimal SalesPrice { get; set; }
        public override string ToString()
        {
            return SalesId + Delimiter + SalesCategoryId
                + Delimiter + SalesUnitsId + Delimiter + SalesQuantity
                + Delimiter + SalesPrice; 
        }
    }
}
