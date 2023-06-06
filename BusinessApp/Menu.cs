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
        GoodsRepository goodsRepository = new GoodsRepository(Console.WriteLine);
        UnitsRepository unitsRepository = new UnitsRepository(Console.WriteLine);
        SalesRepository salesRepository = new SalesRepository(Console.WriteLine);
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
            Console.Write("->");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1"://Создать новый Goods
                    Goods goods = new Goods();
                    StartGoodsExecution(goods);
                    StartMenu();
                    break;
                case "2"://Считать Goods
                    ReadGoodsFile();
                    StartMenu();
                    break;
                case "3":
                    Units units = new Units();
                    StartUnitsExecution(units);
                    StartMenu();
                    break;
                case "4":
                    ReadUnitsFile();
                    StartMenu();
                    break;
                case "5":
                    Sales sales = new Sales();
                    StartSalesExecution(sales);
                    StartMenu();
                    break;
                default:
                    Console.WriteLine("There's no such option! Please try again");
                    Console.WriteLine("\n");
                    Console.Clear();
                    StartMenu();
                    break;
            }
        }
        //Запуск работы c Goods
        private void StartGoodsExecution(Goods goods)
        {
            bool exist = false;
            Console.Write("Enter the name of product: ");
            goods.GoodsName = Console.ReadLine();
            exist = goodsRepository.GoodsRepositoryExectution(goods, exist);
            if (exist==true)
            {
                GoodsMatchCheckTrue(goods);
            }
            else
            {
                Console.WriteLine("Position created successfully!\n Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        //Такой Goods уже существует
        public void GoodsMatchCheckTrue(Goods goods)
        {
            Console.WriteLine($"{goods.GoodsName} already exists. Do you want to try again? y/n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    StartGoodsExecution(goods);
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
        private void StartUnitsExecution(Units units)
        {
            bool exist = false;
            Console.Write("Enter the type of Units: ");
            units.UnitsName = Console.ReadLine();
            exist = unitsRepository.UnitsRepositoryExectution(units, exist);
            if (exist == true)
            {
                UnitsMatchCheckTrue(units);
            }
            else
            {
                Console.WriteLine("Position created successfully!\n Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        //Такой Units уже существует
        public void UnitsMatchCheckTrue(Units units)
        {
            Console.WriteLine($"{units.UnitsName} already exists. Do you want to try again? y/n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    StartUnitsExecution(units);
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
        //Запуск работы c Sales
        private void StartSalesExecution(Sales sales)
        {
            string line = "";
            line = GettingSalesID(sales, line);
            line = GettingSalesGoodsIDFromGoods(sales, line);
            line = GettingSalesUnitsIDFromUnits(sales, line);
            line = GettingSalesQuantity(sales, line);
            line = GettingSalesPrice(sales, line);
            line = salesRepository.WriteSalesFile(sales, line);

        }
        //ID покупки
        private string GettingSalesID(Sales sales, string line)
        {
            string result;
            salesRepository.CheckSalesID(sales);
            result = Convert.ToString(sales.SalesID);
            result += ";";
            line += result;
            return line;
        }
        //Вписываем в строку имя товара(Goods).
        //Если товар есть в базе, берём его ID, если нет то создаем новый товар и продолжаем работу 
        private string GettingSalesGoodsIDFromGoods(Sales sales, string line)
        {
            string result;
            int GoodsID = 0;
            Goods goods = new Goods();
            Console.Write("Enter the name of product: ");
            goods.GoodsName = Console.ReadLine();
            GoodsID = CheckingGoodsID(GoodsID, goods);
            if (GoodsID == 0)
            {
                Console.WriteLine($"{goods.GoodsName} doesn't exist in the database. Creating new product...\n");
                goodsRepository.ExecuteGoods(goods);
                sales.SalesGoodsID = goods.GoodsID;
            }
            else sales.SalesGoodsID = GoodsID;
            result = Convert.ToString(sales.SalesGoodsID);
            result += ";";
            line += result;
            return line;
        }
        private int CheckingGoodsID(int GoodsID,Goods goods)
        {
            bool exist = false;
            
            exist = goodsRepository.GoodsMatchCheck(goods);
            if (exist==true)
            {
                GoodsID = goods.GoodsID;
            }
            return GoodsID;
        }
        //Вписываем в строку тип товара(Units).
        //Если тип товара есть в базе, берём его ID, если нет то создаем новый и продолжаем работу 
        private string GettingSalesUnitsIDFromUnits(Sales sales, string line)
        {
            string result;
            int UnitsID = 0;
            Units units = new Units();
            Console.Write("Enter the type of product: ");
            units.UnitsName = Console.ReadLine();
            UnitsID = CheckingUnitsID(UnitsID, units);
            if (UnitsID == 0)
            {
                Console.WriteLine($"{units.UnitsName} doesn't exist in the database. Creating new type of product...\n");
                unitsRepository.ExecuteUnits(units);
                sales.SalesUnitsID = units.UnitsID;
            }
            else sales.SalesUnitsID = UnitsID;
            result = Convert.ToString(sales.SalesUnitsID);
            result += ";";
            line += result;
            return line;
        }
        private int CheckingUnitsID(int UnitsID, Units units)
        {
            bool exist = false;
            
            exist = unitsRepository.UnitsMatchCheck(units);
            if (exist == true)
            {
                UnitsID = units.UnitsID;
            }
            return UnitsID;
        }
        //Получаем со строки Quantity
        private string GettingSalesQuantity(Sales sales, string line)
        {
            string result;
            Console.Write("Enter the quantity of units: ");
            sales.SalesQuantity = Convert.ToInt32(Console.ReadLine());
            result = Convert.ToString(sales.SalesQuantity);
            result += ";";
            line += result;
            return line;
        }
        //Получаем со строки Price
        private string GettingSalesPrice(Sales sales, string line)
        {
            string result;
            Console.Write("Enter the quantity of units: ");
            sales.SalesPrice = Convert.ToInt32(Console.ReadLine());
            result = Convert.ToString(sales.SalesPrice);
            result += ";";
            line += result;
            return line;
        }
    }
}
