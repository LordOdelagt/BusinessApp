using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Price : Common
    {
        public int PriceId { get; set; }
        public int PriceCategoryId { get; set; }
        public int PriceUnitsId { get; set; }
        public decimal PriceTotal { get; set; }
        public override string ToString()
        {
            return 
                PriceId.ToString()      + Delimiter + 
                PriceCategoryId.ToString() + Delimiter + 
                PriceUnitsId.ToString() + Delimiter + 
                PriceTotal.ToString();
        }
    }
}
