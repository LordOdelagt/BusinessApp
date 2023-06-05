using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GoodsRepository
    {
        private static int Counter = 0;
        readonly public string FilePath = "Data/GoodsData.csv";
        readonly Action<string> warningnMessage;
        //Конструкция для сообщения об ошибке
        public GoodsRepository(Action<string> warningMessage)
        {
            this.warningnMessage = warningMessage;
        }
        //Начало работы с Goods
        public bool GoodsRepositoryExectution(Goods goods, bool exist)
        {
            if (GoodsMatchCheck(goods) == false)
            {
                ExecuteGoods(goods);
                return exist;
            }
            else
            {
                exist = true;
                return exist;
            }
        }
        public void ExecuteGoods(Goods goods)
        {
            CheckGoodsID();
            WriteGoodsFile(goods);
        }
        //Запись нового элемента
        public void WriteGoodsFile(Goods goods)
        {
            goods.GoodsID = Counter;
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    writer.Write($"\n{goods.GoodsID};{goods.GoodsName}");
                }
                Counter++;
            }
            catch (IOException e)
            {
                warningnMessage("An error occurred while creating the CSV file: " + e.Message);
            }
        }
        //Получение количества строк. ID будущего элемента = Counter. 
        private void CheckGoodsID()
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
        }
        //Проверка на совпадения
        public bool GoodsMatchCheck(Goods goods, bool exist = false)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');
                    if (values[1].Contains(goods.GoodsName)) //перепроверка второго элемента масива, который является именем 
                    {
                        goods.GoodsID = Convert.ToInt32(values[0]);
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }
    }
}
