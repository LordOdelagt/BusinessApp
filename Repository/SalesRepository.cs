using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Repository
{
    public class SalesRepository
    {
        private static int Counter = 0;
        readonly public string FilePath = "Data/SalesData.csv";
        readonly Action<string> warningnMessage;
        //Конструкция для сообщения об ошибке
        public SalesRepository(Action<string> warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Получаем ID будущей покупки
        public void CheckSalesID(Sales sales)
        {
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
                sales.SalesID = Counter;
            }
        }
        //Запись нового элемента
        public string WriteSalesFile(Sales sales, string line)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{line}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage("An error occurred while creating the CSV file: " + e.Message);
            }
            return line;
        }
    }
}
