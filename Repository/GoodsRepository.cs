using Entity;
using System;
using System.Collections;
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
        public Goods CreateGoods(string name)
        {
            var Goods = new Goods { Id = CheckGoodsID(), Name = name };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{Goods.ToString()}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return Goods;
        }
        //Получение количества строк. ID будущего элемента = Counter. 
        private int CheckGoodsID()
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
        //Возвращает объект Goods по введенному id
        public Goods SearchGoodsByID(int id)
        {
            using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Goods Goods = GetFromCsv(line);//теперь работает через getfromCsv
                    if (Goods.Id == id)
                    {
                        return Goods; //Было через return new Goods. Исправлено 
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
                    Goods Goods = GetFromCsv(line);
                    string GoodsName = Goods.Name;
                    if (GoodsName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Goods;
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
                    Goods Goods = GetFromCsv(line);
                    list.Add(Goods);
                }
            }
            return list;
        }
        public Goods GetFromCsv(string line)
        {
            if (line != null)
            {
                string[] values = line.Split(Delimiter);
                return new Goods 
                {
                    Id = Convert.ToInt32(values[(int)ProductEnum.Id]),
                    Name = Convert.ToString(values[(int)ProductEnum.Name])
                };
            }
            return null;
        }
    }
}
