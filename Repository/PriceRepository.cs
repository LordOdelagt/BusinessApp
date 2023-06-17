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

        public Price CreatePrice(Goods goods, Units units, decimal total)
        {
            var price = new Price
            {
                PriceId = CheckPriceId(),
                PriceGoodsId = goods.Id,
                PriceUnitsId = units.Id,
                PriceTotal = total
            };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{price.ToString()}");
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
                    Price price = GetFromString(line);
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
                    Price price = GetFromString(line);
                    if (price.PriceId == id)
                    {
                        return price;
                    }
                }
            }
            return null;
        }

        public Price SearchPriceByMatch(Goods goods, Units units)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Price price = GetFromString(line);
                    if (price.PriceGoodsId == goods.Id && price.PriceUnitsId == units.Id)
                    {
                        return price;
                    }
                }
            }
            return null;
        }

        public Price GetFromString(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Price
                {
                    PriceId = Convert.ToInt32(values[(int)PriceEnum.PriceId]),
                    PriceGoodsId = Convert.ToInt32(values[(int)PriceEnum.GoodsId]),
                    PriceUnitsId = Convert.ToInt32(values[(int)PriceEnum.UnitsId]),
                    PriceTotal = Convert.ToDecimal(values[(int)PriceEnum.PriceTotal])
                };
            }
            return null;
        }
    }
}
