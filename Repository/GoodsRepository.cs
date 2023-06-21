using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GoodsRepository : Common, IGoodsRepository
    {
        private static int Counter = 0;

        readonly IExceptionLog warningnMessage;

        public string FilePath => "Data/GoodsData.csv";

        //Конструкция для сообщения об ошибке
        public GoodsRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Запись нового элемента
        public Goods CreateGoods(string name, int categoryId, int unitsId, int quantity)
        {
            var goodsId = GetGoods().Max(g => g.Id) + 1;
            var goods = new Goods 
            {
                Id = goodsId,
                Name = name,
                CategoryId = categoryId,
                UnitsId = unitsId,
                GoodsQuantity = quantity
            };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{goods}");
                }
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return goods;
        }
        
        //Возвращает объект Goods по введенному id
        public Goods SearchGoodsByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Goods goods = GetFromCsv(line);
                    if (goods.Id == id)
                    {
                        return goods; //Было через return new Category. Исправлено 
                    }
                }
            }
            return null;
        }
        //Возравщение обьекта Goods по имени 
        public Goods GetGoodsByName(string? name)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Goods goods = GetFromCsv(line);
                    string goodsName = goods.Name;
                    if (goodsName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return goods;
                    }
                }
            }
            return null;
        }

        public List<Goods> GetGoods()
        {
            List<Goods> list = new List<Goods>();
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Goods goods = GetFromCsv(line);
                    list.Add(goods);
                }
            }
            return list;
        }

        public void Delete(int id)
        {
            var list = GetGoods();
            var goods = list.SingleOrDefault(goods => goods.Id == id);
            var goods11 = list.Remove(goods);
            File.Delete(FilePath);
            foreach (var item in list)
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"{item}");
                }
            }
        }
        public Goods GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Goods
                {
                    Id = Convert.ToInt32(values[(int)GoodsEnum.Id]),
                    Name = Convert.ToString(values[(int)GoodsEnum.Name]),
                    CategoryId = Convert.ToInt32(values[(int)GoodsEnum.CategoryId]),
                    UnitsId = Convert.ToInt32(values[(int)GoodsEnum.UnitsId]),
                    GoodsQuantity = Convert.ToInt32(values[(int)GoodsEnum.Quantity]),
                };
            }
            return null;
        }
    }
}
