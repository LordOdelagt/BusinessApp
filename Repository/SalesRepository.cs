using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entity;

namespace Repository
{
    public class SalesRepository : Common, ISalesRepository
    {
        private static int Counter = 0;
        public string FilePath => "Data/SalesData.csv";

        readonly IExceptionLog warningnMessage;
        //Конструкция для сообщения об ошибке
        public SalesRepository(IExceptionLog warningMessage) => this.warningnMessage = warningMessage;
        //Получаем ID будущей покупки
        public int CheckSalesID()
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
        public Sales SearchSalesByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Sales sales = GetFromCsv(line);
                    if (sales.SalesId == id)
                    {
                        return sales;
                    }
                }
            }
            return null;
        }
        //Временное решение?
        //Будет дополняться по мере добавления новых Entity
        public Sales CreateSales(Goods Goods, Units units, Price price, int quantity)
        {
            //Потенциально надо как-то упростить 
            var sales = new Sales
            {
                SalesId = CheckSalesID(),
                SalesGoodsId = Goods.Id,
                SalesUnitsId = units.Id,
                SalesQuantity = quantity,
                SalesPrice = price.PriceTotal * quantity
            };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{sales}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                return null;
            }
            return sales;
        }

        public List<Sales> GetSales()
        {
            List<Sales> list = new List<Sales>();
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Sales sales = GetFromCsv(line);
                    list.Add(sales);//Нужно ли тут добавить перепроверку на null?
                }
            }
            return list;
        }


        public Sales GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Sales
                {
                    SalesId = Convert.ToInt32(values[(int)SalesEnum.Id]),
                    SalesGoodsId = Convert.ToInt32(values[(int)SalesEnum.GoodsId]),
                    SalesUnitsId = Convert.ToInt32(values[(int)SalesEnum.UnitsId]),
                    SalesQuantity = Convert.ToInt32(values[(int)SalesEnum.Quantity]),
                    SalesPrice = Convert.ToDecimal(values[(int)SalesEnum.Price])
                };
            }
            return null;
        }
    }
}
