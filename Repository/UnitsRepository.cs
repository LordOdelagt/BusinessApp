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
            var units = new Units { Id = CheckUnitsID(), Name = name };
            try
            {
                Counter++;
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{units.ToString()}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return units;
        }
        public Units SearchUnitsByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Units units = GetFromString(line);
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
                    Units units = GetFromString(line);
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
                    Units units = GetFromString(line);
                    list.Add(units);//Нужно ли тут добавить перепроверку на null?
                }
            }
            return list;
        }

        public Units GetFromString(string line)
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
