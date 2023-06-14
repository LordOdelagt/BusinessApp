using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitsRepository : IUnitsRepository
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
                }
            }
            return Counter;
        }
        public Units CreateUnits(string? name)
        {
            var units = new Units { Id = CheckUnitsID(), Name = name };
            try
            {
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
            if (id > 0)
            {
                int i = 0;
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == id)
                        {
                            string name = values[(int)ProductEnum.Name];
                            return new Units { Id = id, Name = name };
                        }
                        i++;
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
                    //Units units = (Units)Product.GetFromCsv(line, name);
                    //if (units != null)
                    //{
                    //    return units;
                    //}
                    string[] values = line.Split(';');
                    if (values[(int)ProductEnum.Name].Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        int id = Convert.ToInt32(values[(int)ProductEnum.Id]);
                        return new Units { Id = id, Name = name };
                    }
                }
            }
            return null;
        }

        
    }
}
