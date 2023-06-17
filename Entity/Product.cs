using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entity
{
    public class Product : Common
    {
        public string? Name { get; set; }   
        public int Id { get; set; }

        public override string ToString()
        {
            return Id.ToString() + Delimiter + Name;
        }
        //public static Product GetFromCsv(string line, string name)
        //{
        //    string[] values = line.Split(';');
        //    if (values[(int)ProductEnum.Name].Equals(name, StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        int id = Convert.ToInt32(values[(int)ProductEnum.Id]);
        //        return new Product { Id = id, Name = name };
        //    }
        //    return null;
        //}
        //Мой вариант 
        public Product GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(';');
                int id = Convert.ToInt32(values[(int)ProductEnum.Id]);
                string name = Convert.ToString(values[(int)ProductEnum.Name]);
                return new Product { Id = id, Name = name };
            }
            return null;
        }
    }
}
