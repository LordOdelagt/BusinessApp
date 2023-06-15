using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Repository;

namespace BusinessApp
{
    public class Menu
    {
        IGoodsRepository goodsRepository = new GoodsRepository(new ExceptionLogToConsole());
        IUnitsRepository unitsRepository = new UnitsRepository(new ExceptionLogToConsole());
        ISalesRepository salesRepository = new SalesRepository(new ExceptionLogToConsole());
        public void StartMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome! This is Business App.\n");
            Console.WriteLine("Please, choose the option:");
            Console.WriteLine("\n1. Create new Goods\n2. Check Goods\n3. Create new Units\n4. Check Units\n5. Create new Sales\n6. Check sales\n0. Exit");
            Navigation();
        }
        private void Navigation()
        {
            Console.Write(" - ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1"://Создать новый Goods
                    StartGoodsExecution();
                    StartMenu();
                    break;
                case "2"://Считать Goods
                    ReadGoodsFile();
                    StartMenu();
                    break;
                case "3":
                    StartUnitsExecution();
                    StartMenu();
                    break;
                case "4":
                    ReadUnitsFile();
                    StartMenu();
                    break;
                case "5":
                    StartSalesExecution();
                    StartMenu();
                    break;
                case "6":
                    ReadSalesFile();
                    StartMenu();
                    break;
                default:
                    Console.WriteLine("There's no such option! Please try again. Press Enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
            }
        }
        //Запуск работы c Goods
        private void StartGoodsExecution()
        {
            bool exist = true;
            string? name = EnterName();
            var goods = goodsRepository.GetGoodsByName(name);
            if (goods == null)
            {
                goodsRepository.CreateGoods(name);
                exist = false;
            }
            if (exist)
            {
                GoodsMatchCheckTrue(name);
            }
            else
            {
                Console.WriteLine("Position created successfully!\n Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        

//Запуск работы c Goods из Sales
//public void StartGoodsExecutionFromSales(Goods goods)
//{
//    bool exist = false;
//    exist = goodsRepository.GoodsRepositoryExectution(goods, exist);
//    if (exist == false)//Такой Goods не существует, создаем новый и продолжаем
//    {
//        Console.WriteLine($"{goods.Name} doesn't exist in the database. Creating new product...\n");
//    }
//        //Или такой Goods существует, продолжаем
//}
//Такой Goods уже существует
        private void GoodsMatchCheckTrue(string name)
        {
            Console.WriteLine($"{name} already exists. Do you want to try again? y/n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    //bool FromSales = false;
                    StartGoodsExecution();
                    break;
                case "n":
                    Console.Clear();
                    return;
            }
        }
        //Читает информацию с файла Goods.
        private void ReadGoodsFile()
        {
            Console.Clear();
            Console.WriteLine("The existing positions are: \n");
            try
            {
                using (StreamReader reader = new StreamReader(goodsRepository.FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        foreach (var item in values)
                        {
                            Console.Write($"{item} \t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
            Console.Clear();
        }
        //Запуск работы c Units
        private void StartUnitsExecution()
        {
            bool exist = true;
            string? name = EnterName();
            var units = unitsRepository.GetUnitsByName(name);
            if (units == null)
            {
                unitsRepository.CreateUnits(name);
                exist = false;
            }
            if (exist)
            {
                UnitsMatchCheckTrue(name);
            }
            else
            {
                Console.WriteLine("Position created successfully!\n Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        private void UnitsMatchCheckTrue(string name)
        {
            Console.WriteLine($"{name} already exists. Do you want to try again? y/n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    StartUnitsExecution();
                    break;
                case "n":
                    Console.Clear();
                    return;
            }
        }
        //Читает информацию с файла Units
        private void ReadUnitsFile()
        {
            Console.Clear();
            Console.WriteLine("The existing positions are: \n");
            try
            {
                using (StreamReader reader = new StreamReader(unitsRepository.FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        foreach (var item in values)
                        {
                            Console.Write($"{item} \t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
            Console.Clear();
        }

        private void StartSalesExecution()
        {
            int goodsId;
            int unitsId;
            int quantity;
            decimal price;
            bool exist = true;
            Console.Write("Enter the name of goods");
            string? name = EnterName();
            var goods = goodsRepository.GetGoodsByName(name);
            if (goods == null)
            {
                Console.WriteLine($"{name} doesn't exist in the database. Adding to database...\n");
                goodsRepository.CreateGoods(name);
            }
            goodsId = goods.Id;
            Console.Write("Enter the type of goods");
            name = EnterName();
            var units = unitsRepository.GetUnitsByName(name);
            if (units == null)
            {
                Console.WriteLine($"{name} doesn't exist in the database. Adding to database...\n");
                unitsRepository.CreateUnits(name);
            }
            unitsId = units.Id;
            Console.Write("Enter quantity: ");
            quantity = Convert.ToInt32(Console.ReadLine());
            var sales = salesRepository.CreateSales(goodsId, unitsId, quantity);
            if (sales != null)
            {
                salesRepository.CreateSales(goodsId, unitsId, quantity);
                Console.WriteLine("Position created successfully!\n Press Enter to continue...");
            }
            else Console.WriteLine("Something went wrong...");
            Console.ReadLine();
            Console.Clear();
        }
        private void ReadSalesFile()
        {
            Console.Clear();
            Console.WriteLine("The existing positions are: \n");
            try
            {
                using (StreamReader reader = new StreamReader(salesRepository.FilePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        var goods = goodsRepository.SearchGoodsByID(Convert.ToInt32(values[(int)ProductEnum.Id]));
                        values[(int)SalesEnum.GoodsId] = Convert.ToString(goods.Name);
                        var units = unitsRepository.SearchUnitsByID(Convert.ToInt32(values[(int)ProductEnum.Id]));
                        values[(int)SalesEnum.UnitsId] = Convert.ToString(units.Name);
                        foreach (var item in values)
                        {
                            Console.Write($"{item}");
                            Console.Write($"\t");
                            Console.Write($"\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
            Console.Clear();
        }

        private string? EnterName()
        {
            Console.Write(" - ");
            return Console.ReadLine();
        }
        private int EnterId()
        {
            Console.Write(" - ");
            return Convert.ToInt32(Console.ReadLine());
        }
        public string? EnterGoodsName()
        {
            Console.Write("Enter the name of product: ");
            return Console.ReadLine();
        }
        public string? EnterUnitsName()
        {
            Console.Write("Enter the type of product: ");
            return Console.ReadLine();
        }
        public void EnterSalesQuantity(Sales sales)
        {
            Console.Write("Enter quantity: ");
            sales.SalesQuantity = Convert.ToInt32(Console.ReadLine());
        }
    }
}
