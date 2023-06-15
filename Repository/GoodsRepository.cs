using Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GoodsRepository : IGoodsRepository
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
            var goods = new Goods { Id = CheckGoodsID(), Name = name };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{goods.ToString()}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage.Log("An error occurred while creating the CSV file: " + e.Message);
                throw new IOException();
            }
            return goods;
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
                }
            }
            return Counter;
        }
        //Возвращает GoodsName по GoodsID
        public Goods SearchGoodsByID(int id)
        {
            if (id > 0)
            {
                int i = 0;
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == id)
                        {
                            string name = values[(int)ProductEnum.Name];
                            return new Goods { Id = id, Name = name };
                        }
                        i++;
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
                    //Goods goods = (Goods)Product.GetFromCsv(line, name);
                    //if (goods != null)
                    //{
                    //    return goods;
                    //}
                    string[] values = line.Split(';');
                    if (values[(int)ProductEnum.Name].Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        int id = Convert.ToInt32(values[(int)ProductEnum.Id]);
                        return new Goods { Id = id, Name = name };
                    }
                }
            }
            return null;
        }

        
    }
}
