using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Units : Product
    {
        //Неявный метод для сравнения. Не работает без IComparable<Units>
        //public int CompareTo(Units? other)
        //{
        //    return this.Id > other?.Id ? this.Id : other.Id;
        //}
    }
}
