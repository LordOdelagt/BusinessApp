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
                //case "5":
                //    CreateNewSales();
                //    StartMenu();
                //    break;
                //case "6":
                //    ReadSalesFile();
                //    StartMenu();
                //    break;
                default:
                    Console.WriteLine("There's no such option! Please try again");
                    Console.WriteLine("\n");
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
        public void GoodsMatchCheckTrue(string name)
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
        //Запуск работы c Units из Sales
        //private void StartUnitsExecutionFromSales(Units units)
        //{
        //    bool exist = false;
        //    exist = unitsRepository.UnitsRepositoryExectution(units, exist);
        //    if (exist == false)//Такой Units не существует, создаем новый и продолжаем
        //    {
        //        Console.WriteLine($"{units.UnitsName} doesn't exist in the database. Creating new product...\n");
        //    }
        //    //Или такой Units существует, продолжаем
        //}
        //Такой Units уже существует
        public void UnitsMatchCheckTrue(string name)
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

        //private void CreateNewSales()
        //{
        //    Sales sales = new Sales();
        //    string line = "";
        //    line = StartCreateSalesExecution(sales, line);
        //    salesRepository.WriteSalesFile(sales, line);
        //}
        //private void ReadSalesFile()
        //{
        //    string readResult = "";
        //    Console.Clear();
        //    Console.WriteLine("Sales data: \n");
        //    using (StreamReader reader = new StreamReader(salesRepository.FilePath, Encoding.UTF8))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            Sales sales = new Sales();
        //            string line = reader.ReadLine();
        //            readResult = StartReadSalesExecution(sales, line, readResult);
        //            string[] values = readResult.Split(';');
        //            foreach (var item in values)
        //            {
        //                Console.Write($"{item}");
        //                Console.Write($"\t");
        //            }
        //            Console.WriteLine();
        //            readResult = "";
        //        }
        //    }
        //    Console.WriteLine("\nPress Enter to return...");
        //    Console.ReadLine();
        //}
        //Запуск работы c Sales.
        //В конечном итоге получаем полноценный объект sales со всеми параметрами
        //плюс строку дублирующую его параметры для потенциального чтения и записи
        //private string StartCreateSalesExecution(Sales sales, string line)
        //{
        //    line = CreateSalesID(sales, line);
        //    line = CreateSalesGoodsIDFromGoods(sales, line);
        //    line = CreateSalesUnitsIDFromUnits(sales, line);
        //    line = CreateSalesQuantity(sales, line);
        //    line = CreateSalesPrice(sales, line);
        //    return line;
        //}
        //private string StartReadSalesExecution(Sales sales, string line, string readResult)
        //{
        //    readResult = ReadSalesID(sales, line, readResult);
        //    readResult = ReadSalesGoodsIDFromGoods(sales, line, readResult);
        //    readResult = ReadSalesUnitsIDFromUnits(sales, line, readResult);
        //    readResult = ReadSalesQuantity(sales, line, readResult);
        //    readResult = ReadSalesPrice(sales, line, readResult);
        //    return readResult;
        //}

        ////ID покупки Create
        //private string CreateSalesID(Sales sales, string line)
        //{
        //    string buffer;
        //    salesRepository.CheckSalesID(sales);
        //    sales.SalesID++;
        //    buffer = Convert.ToString(sales.SalesID);
        //    buffer += ";";
        //    line += buffer;
        //    return line;
        //}
        ////ID покупки Read
        //private string ReadSalesID(Sales sales, string line, string readResult)
        //{
        //    string buffer;
        //    string[] values = line.Split(';');
        //    sales.SalesID = Convert.ToInt32(values[0]);
        //    buffer = Convert.ToString(sales.SalesID);
        //    buffer += ";";
        //    readResult += buffer;
        //    return readResult;
        //}
        ////Вписываем в строку имя товара(Goods).
        ////Если товар есть в базе, берём его ID, если нет то создаем новый товар и продолжаем работу 
        //private string CreateSalesGoodsIDFromGoods(Sales sales, string line)
        //{
        //    string buffer;
        //    Goods goods = new Goods();
        //    //StartGoodsExecutionFromSales(goods);
        //    sales.SalesGoodsID = goods.Id;
        //    buffer = Convert.ToString(sales.SalesGoodsID);
        //    buffer += ";";
        //    line += buffer;
        //    return line;
        //}

        ////Действия по чтению Goods
        //private string ReadSalesGoodsIDFromGoods(Sales sales, string line, string readResult)
        //{
        //    string buffer;
        //    Goods goods = new Goods();
        //    string[] values = line.Split(';');
        //    sales.SalesGoodsID = Convert.ToInt32(values[1]);
        //    string[] values2 = line.Split(';');
        //    var id = Convert.ToInt32(values2[1]);
        //    goodsRepository.SearchGoodsByID(id);//Получаем GoodsName исходя из номера ID
        //    buffer = goods.Name;
        //    buffer += ";";
        //    readResult += buffer;
        //    return readResult;
        //}
        //private string CreateSalesUnitsIDFromUnits(Sales sales, string line)
        //{
        //    string buffer;
        //    Units units = new Units();
        //    StartUnitsExecutionFromSales(units);
        //    sales.SalesUnitsID = units.UnitsID;
        //    buffer = Convert.ToString(sales.SalesUnitsID);
        //    buffer += ";";
        //    line += buffer;
        //    return line;
        //}
        //private string ReadSalesUnitsIDFromUnits(Sales sales, string line, string readResult)
        //{
        //    string buffer;
        //    Units units = new Units();
        //    string[] values = line.Split(';');
        //    sales.SalesUnitsID = Convert.ToInt32(values[2]);
        //    string[] values2 = line.Split(';');
        //    units.UnitsID = Convert.ToInt32(values2[2]);
        //    unitsRepository.SearchUnitsByID(id);//Получаем UnitsName исходя из номера ID
        //    buffer = units.UnitsName;
        //    buffer += ";";
        //    readResult += buffer;
        //    return readResult;
        //}
        ////Получаем со строки Quantity
        //private string CreateSalesQuantity(Sales sales, string line)
        //{
        //    string buffer;
        //    EnterSalesQuantity(sales);
        //    buffer = Convert.ToString(sales.SalesQuantity);
        //    buffer += ";";
        //    line += buffer;
        //    return line;
        //}
        //private string ReadSalesQuantity(Sales sales, string line, string readResult)
        //{
        //    string buffer;
        //    string[] values = line.Split(';');
        //    sales.SalesQuantity = Convert.ToInt32(values[3]);
        //    buffer = Convert.ToString(sales.SalesQuantity);
        //    buffer += ";";
        //    readResult += buffer;
        //    return readResult;
        //}
        ////Получаем со строки Price
        //private string CreateSalesPrice(Sales sales, string line)
        //{
        //    string buffer;
        //    EnterSalesPrice(sales);
        //    buffer = Convert.ToString(sales.SalesPrice);
        //    buffer += ";";
        //    line += buffer;
        //    return line;
        //}
        //private string ReadSalesPrice(Sales sales, string line, string readResult)
        //{
        //    string buffer;
        //    string[] values = line.Split(';');
        //    sales.SalesPrice = Convert.ToInt32(values[4]);
        //    buffer = Convert.ToString(sales.SalesPrice);
        //    buffer += ";";
        //    readResult += buffer;
        //    return readResult;
        //}
        private string? EnterName()
        {
            Console.Write("Enter the name: ");
            return Console.ReadLine();
        }
        private int EnterId()
        {
            Console.Write("Enter Id: ");
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
        public void EnterSalesPrice(Sales sales)
        {
            Console.Write("Enter the price: ");
            sales.SalesPrice = Convert.ToInt32(Console.ReadLine());
        }
    }
}
