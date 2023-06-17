using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUnitsRepository
    {
        string FilePath { get; }

        Units SearchUnitsByID(int id);
        Units GetUnitsByName(string? name);
        Units CreateUnits(string? name);
        List<Units> GetUnits();
        Units GetFromString(string line);
    }
}
