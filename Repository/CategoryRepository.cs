using Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : Common, ICategoryRepository
    {
        private static int Counter = 0;

        readonly IExceptionLog warningnMessage;

        public string FilePath => "Data/CategoryData.csv";

        //Конструкция для сообщения об ошибке
        public CategoryRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Запись нового элемента
        public Category CreateCategory(string name)
        {
            var CategoryId = GetCategory().Max(g => g.Id) + 1;
            var Category = new Category { Id = CategoryId, Name = name };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{Category}");
                }
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return Category;
        }
        //Получение количества строк. ID будущего элемента = Counter. 
        private int CheckCategoryID()
        {
            if (Counter == 0)
            {
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (reader.ReadLine() != null)
                    {
                        Counter++;
                    }
                    Counter++;
                }
            }
            return Counter;
        }
        //Возвращает объект Category по введенному id
        public Category SearchCategoryByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Category Category = GetFromCsv(line);//теперь работает через getfromCsv
                    if (Category.Id == id)
                    {
                        return Category; //Было через return new Category. Исправлено 
                    }
                }
            }
            return null;
        }
        //Возравщение обьекта Category по имени 
        public Category GetCategoryByName(string? name)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Category Category = GetFromCsv(line);
                    string CategoryName = Category.Name;
                    if (CategoryName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Category;
                    }
                }
            }
            return null;
        }

        public List<Category> GetCategory()
        {
            List<Category> list = new List<Category>();
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Category Category = GetFromCsv(line);
                    list.Add(Category);
                }
            }
            return list;
        }

        public void Delete(int id)
        {
            var list = GetCategory();
            var Category = list.SingleOrDefault(Category => Category.Id == id);
            var Category1 = list.Remove(Category);
            File.Delete(FilePath);
            var myFile = File.Create(FilePath);
            myFile.Close();
            foreach (var item in list)
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"{item}");
                }
            }
        }
        public Category GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Category 
                {
                    Id = Convert.ToInt32(values[(int)ProductEnum.Id]),
                    Name = Convert.ToString(values[(int)ProductEnum.Name])
                };
            }
            return null;
        }
    }
}
