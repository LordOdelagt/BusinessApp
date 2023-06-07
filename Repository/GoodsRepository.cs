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
        readonly public string FilePath = "Data/GoodsData.csv";
        readonly IExceptionLog warningnMessage;
        //Конструкция для сообщения об ошибке
        public GoodsRepository(IExceptionLog warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Начало работы с Goods
        
        //Запись нового элемента
        public Goods CreateGoods(string name)
        {
            var goods = new Goods { Id = CheckGoodsID(), Name = name };
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{goods.ToCsv()}");
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
            if (goods.Id > 0)
            {
                int i = 0;
                using (StreamReader reader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        if (i == goods.Id)
                        {
                            goods.Name = values[1];
                            break;
                        }
                        i++;
                    }
                }
            }
        }
        //Проверка на совпадения
        public Goods GetGoodsByName(string? name)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    if (values[1].Equals(name, StringComparison.InvariantCultureIgnoreCase)) //перепроверка второго элемента масива, который является именем 
                    {
                        int id = Convert.ToInt32(values[0]);
                        return new Goods { Id = id, Name = name };
                    }
                }
            }
            return null;
        }

        
    }
}
