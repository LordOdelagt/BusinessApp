using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICategoryRepository
    {
        string FilePath { get; }

        Category SearchCategoryByID(int id);
        Category GetCategoryByName(string? name);
        Category CreateCategory(string name);
        List<Category> GetCategory();
        void Delete(int id);
    }
}
