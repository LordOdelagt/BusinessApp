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
        //Будет дополняться по мере добавления новых Entity
        public Sales CreateSales(int goodsId, int categoryId, int unitsId, decimal price, int quantity)
        {
            int saleId = GetSales().Max(s => s.SalesId) + 1;
            var sales = new Sales
            {
                SalesId = saleId,
                SalesGoodsId = goodsId,
                SalesCategoryId = categoryId,
                SalesUnitsId = unitsId,
                SalesQuantity = quantity,
                SalesPrice = price * quantity
            };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{sales}");
                }
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

        public void Delete(int id)
        {
            var list = GetSales();
            var sale = list.SingleOrDefault(sale => sale.SalesId == id);
            var sale1 = list.Remove(sale);
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
        public Sales GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Sales
                {
                    SalesId = Convert.ToInt32(values[(int)SalesEnum.Id]),
                    SalesCategoryId = Convert.ToInt32(values[(int)SalesEnum.CategoryId]),
                    SalesUnitsId = Convert.ToInt32(values[(int)SalesEnum.UnitsId]),
                    SalesQuantity = Convert.ToInt32(values[(int)SalesEnum.Quantity]),
                    SalesPrice = Convert.ToDecimal(values[(int)SalesEnum.Price])
                };
            }
            return null;
        }
    }
}
