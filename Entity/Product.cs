using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Product : Common
    {
        public string Name { get; set; }   
        public int Id { get; set; }

        public override string ToCsv()
        {
            return Id.ToString() + Delimiter + Name;
        }
    }
}
