using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entity
{
    public abstract class Common
    {
        readonly public string Delimiter = ";";
        public abstract string ToCsv();
    }
}
