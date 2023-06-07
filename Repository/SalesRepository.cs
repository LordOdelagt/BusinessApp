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
        //Не готово 
        public void SearchSalesByID(Sales sales)
        {
            if (sales.SalesID > 0)
            {
                int i = 0;
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == sales.SalesID)
                        {
                            sales.SalesID = Convert.ToInt32(values[0]);
                            sales.SalesGoodsID = Convert.ToInt32(values[1]);
                            sales.SalesUnitsID = Convert.ToInt32(values[2]);
                            sales.SalesQuantity = Convert.ToInt32(values[3]);
                            sales.SalesPrice = Convert.ToInt32(values[4]);
                            break;
                        }
                        i++;
                    }
                }
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
