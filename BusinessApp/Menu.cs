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
            string? input = Console.ReadLine();
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
                case "7":
                    int i = EnterInt();
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
            Console.Write("Enter the name of Goods");
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
                foreach (var item in goodsRepository.GetGoods())
                {
                    Console.Write($"{item} \t");
                }
                Console.WriteLine();
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
                foreach (var item in unitsRepository.GetUnits())
                {
                    Console.Write($"{item} \t");
                }
                Console.WriteLine();
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
            Console.WriteLine("Enter the name of goods:");
            string? name = EnterName();
            var goods = goodsRepository.GetGoodsByName(name);
            if (goods == null)
            {
                Console.WriteLine($"{name} doesn't exist in the database. Adding to database...\n");
                goods = goodsRepository.CreateGoods(name);
            }
            Console.WriteLine("Enter the type of goods:");
            name = EnterName();
            var units = unitsRepository.GetUnitsByName(name);
            if (units == null)
            {
                Console.WriteLine($"{name} doesn't exist in the database. Adding to database...\n");
                units = unitsRepository.CreateUnits(name);
            }
            Console.WriteLine("Enter quantity:");
            int quantity = EnterInt();//Done: Сделать функцию на проверку на буквы, null etc.
            var sales = salesRepository.CreateSales(goods, units, quantity);
            if (sales != null)
            {
                Console.WriteLine("\nPosition created successfully!\n Press Enter to continue...");
            }
            else 
                Console.WriteLine("Something went wrong...");
            Console.ReadLine();
            Console.Clear();
        }
        private void ReadSalesFile()
        {
            Console.Clear();
            Console.WriteLine("The existing positions are: \n");
            try
            {
                foreach (var item in salesRepository.GetSales())
                {
                    Console.Write($"{item} \t");
                }
                Console.WriteLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
            Console.Clear();
        }
        
        private string EnterName()
        {
            //TODO: сделать проверку на null (while). Чтобы не было пробелов. Протестить
            Console.Write(" - ");
            string? name = Console.ReadLine();
            while (string.IsNullOrEmpty(name?.Trim()))
            {
                ClearLine();
                Console.Write(" - ");
                name = Console.ReadLine();
            }
            return name;
        }
        private int EnterInt()
        {
            Console.Write(" - ");
            int i = 0;
            while (!int.TryParse(Console.ReadLine(), out i) || int.Equals(i, 0) || i == null)
            {
                ClearLine();
                Console.Write("This is not valid input. Please try again: - ");
            }
            return i;
        }
        //Не работает
        private int Input()
        {
            Console.Write(" - ");
            string input;
            int i=0;
            while (!int.TryParse(input = Convert.ToString(Console.ReadKey()), out i))
            {
                //ClearLine();
                //Console.Write("This is not valid input. Please try again: - ");
                i = Convert.ToInt32(input);
            }
            return i;
        }
        private void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine();
        }
    }
}
