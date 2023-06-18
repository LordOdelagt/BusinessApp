using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitsRepository : Common, IUnitsRepository
    {
        private static int Counter = 0;
        public string FilePath => "Data/UnitsData.csv";
        readonly IExceptionLog warningnMessage;

        //Конструкция для сообщения об ошибке
        public UnitsRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Получение количества строк. ID будущего элемента = Counter. 
        private int CheckUnitsID()
        {
            //TODO: Переработать через List
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
        public Units CreateUnits(string? name)
        {
            var unitId = GetUnits().Max(u=>u.Id)+1;
            
            //var unit1 = GetUnits().SingleOrDefault(u=>u.Id == 3);  //Конструкция по поиску по чему либо(в данном случае по int)
            var units = new Units { Id = unitId, Name = name };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{units}");
                }
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return units;
        }
        public void Delete(int id)
        {
            var list = GetUnits();
            var unit = list.SingleOrDefault(unit => unit.Id == id);
            var unit1 = list.Remove(unit);
            File.Delete(FilePath);
            foreach (var item in list)
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"{item}");
                }
            }
        }
        public Units SearchUnitsByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Units units = GetFromCsv(line);
                    if (units.Id == id)
                    {
                        return units;
                    }
                }
            }
            return null;
        }

        public Units GetUnitsByName(string? name)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Units units = GetFromCsv(line);
                    string unitsName = units.Name;
                    if (unitsName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return units;
                    }
                }
            }
            return null;
        }

        public List<Units> GetUnits()
        {
            List<Units> list = new List<Units>();
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Units units = GetFromCsv(line);
                    list.Add(units);//Нужно ли тут добавить перепроверку на null?
                }
            }
            return list;
        }

        public Units GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Units
                {
                    Id = Convert.ToInt32(values[(int)ProductEnum.Id]),
                    Name = Convert.ToString(values[(int)ProductEnum.Name])
                };
            }
            return null;
        }
    }
}
