using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entity;

namespace Repository
{
    public class SalesRepository : ISalesRepository
    {
        private static int Counter = 0;
        public string FilePath => "Data/SalesData.csv";
        readonly IExceptionLog warningnMessage;
        //Конструкция для сообщения об ошибке
        public SalesRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
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
                    }
                }
            return Counter;
        }
        public Sales SearchSalesByID(int id)
        {
            if (id > 0)
            {
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    int i = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == id)
                        {
                            //Потенциально надо как-то упростить
                            return new Sales 
                            { 
                                SalesId = id, 
                                SalesGoodsId = Convert.ToInt32(values[(int)SalesEnum.GoodsId]),
                                SalesUnitsId = Convert.ToInt32(values[(int)SalesEnum.UnitsId]),
                                SalesQuantity = Convert.ToInt32(values[(int)SalesEnum.Quantity]),
                            };
                        }
                        i++;
                    }
                }
            }
            return null;
        }
        public Sales CreateSales(int goodsId, int unitsId, int quantity)
        {
            //Потенциально надо как-то упростить 
            var sales = new Sales { SalesId = CheckSalesID(), SalesGoodsId = goodsId, SalesUnitsId = unitsId, SalesQuantity = quantity};
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{sales.ToString()}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return sales;
        }
    }
}
