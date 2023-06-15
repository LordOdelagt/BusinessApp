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
        public int SalesGoodsId { get; set; }
        public int SalesUnitsId { get; set; }
        public int SalesQuantity { get; set; }
        //public decimal SalesPrice { get; set; }
        //Деконструктор
        public void Deconstruct(out int ID, out int GoodsID, out int UnitsID, out int Quantity)
        {
            ID = this.SalesId;
            GoodsID = this.SalesGoodsId;
            UnitsID = this.SalesUnitsId;
            Quantity = this.SalesQuantity;
        }
        public override string ToString()
        {
            return SalesId.ToString() + Delimiter + SalesGoodsId
                + Delimiter + SalesUnitsId + Delimiter + SalesQuantity; 
        }
    }
}
