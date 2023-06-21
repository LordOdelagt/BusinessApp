using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Goods : Product
    {
        public int CategoryId { get; set; }
        public int UnitsId { get; set; }
        public int GoodsQuantity { get; set; }
        public override string ToString()
        {
            return
                Id.ToString() + Delimiter +
                Name + Delimiter +
                CategoryId.ToString() + Delimiter +
                UnitsId.ToString() + Delimiter +
                GoodsQuantity.ToString();
        }
    }
}
