using Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PriceRepository : Common, IPriceRepository
    {
        private static int Counter = 0;

        readonly IExceptionLog warningnMessage;

        public string FilePath => "Data/PriceData.csv";

        //Конструкция для сообщения об ошибке
        public PriceRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }

        public Price CreatePrice(int categoryId, int unitsId, decimal total)
        {
            var priceId = GetPrices().Max(p => p.PriceId) + 1;
            var price = new Price
            {
                PriceId = priceId,
                PriceCategoryId = categoryId,
                PriceUnitsId = unitsId,
                PriceTotal = total
            };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{price}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                return null;
            }
            return price;
        }
        public int CheckPriceId()
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

        public List<Price> GetPrices()
        {
            List<Price> list = new List<Price>();
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Price price = GetFromCsv(line);
                    list.Add(price);//Нужно ли тут добавить перепроверку на null?
                }
            }
            return list;
        }

        public Price SearchPriceById(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Price price = GetFromCsv(line);
                    if (price.PriceId == id)
                    {
                        return price;
                    }
                }
            }
            return null;
        }

        public Price SearchPriceByMatch(int CategoryId, int unitsId)
        {
            //TODO: принимать не объекты, а Int
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Price price = GetFromCsv(line);
                    if (price.PriceCategoryId == CategoryId && price.PriceUnitsId == unitsId)
                    {
                        return price;
                    }
                }
            }
            return null;
        }

        public void Delete(int id)
        {
            var list = GetPrices();
            var price = list.SingleOrDefault(price => price.PriceId == id);
            var price1 = list.Remove(price);
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
        public Price GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Price
                {
                    PriceId = Convert.ToInt32(values[(int)PriceEnum.PriceId]),
                    PriceCategoryId = Convert.ToInt32(values[(int)PriceEnum.CategoryId]),
                    PriceUnitsId = Convert.ToInt32(values[(int)PriceEnum.UnitsId]),
                    PriceTotal = Convert.ToDecimal(values[(int)PriceEnum.PriceTotal])
                };
            }
            return null;
        }
    }
}
