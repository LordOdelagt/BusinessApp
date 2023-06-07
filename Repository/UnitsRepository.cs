using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitsRepository
    {
        private static int Counter = 0;
        readonly public string FilePath = "Data/UnitsData.csv";
        readonly Action<string> warningnMessage;
        //Конструкция для сообщения об ошибке
        public UnitsRepository(Action<string> warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        public bool UnitsRepositoryExectution(Units units, bool exist)
        {
            if (UnitsMatchCheck(units) == false)
            {
                ExecuteUnits(units);
                return exist;
            }
            else
            {
                exist = true;
                return exist;
            }
        }
        public void ExecuteUnits(Units units)
        {
            CheckUnitsID();
            WriteUnitsFile(units);
        }
        //Запись нового элемента
        private void WriteUnitsFile(Units units)
        {
            units.UnitsID = Counter;
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{units.UnitsID};{units.UnitsName}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage("An error occurred while creating the CSV file: " + e.Message);
            }
        }
        //Получение количества строк. ID будущего элемента = Counter. 
        private void CheckUnitsID()
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
        }
        public void SearchUnitsByID(Units units)
        {
            if (units.UnitsID > 0)
            {
                int i = 0;
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == units.UnitsID)
                        {
                            units.UnitsName = values[1];
                            break;
                        }
                        i++;
                    }
                }
            }
        }
        //Проверка на совпадения
        public bool UnitsMatchCheck(Units units, bool exist = false)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    if (values[1].Contains(units.UnitsName)) //перепроверка второго элемента масива, который является именем 
                    {
                        units.UnitsID = Convert.ToInt32(values[0]);
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

    }
}
