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
        IPriceRepository priceRepository = new PriceRepository(new ExceptionLogToConsole());
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
                    Console.Clear();
                    ReadGoodsFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "3":
                    StartUnitsExecution();
                    StartMenu();
                    break;
                case "4":
                    Console.Clear();
                    ReadUnitsFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "5":
                    StartSalesExecution();
                    StartMenu();
                    break;
                case "6":
                    Console.Clear();
                    ReadSalesFile();
                    StartMenu();
                    break;
                case "7":
                    StartPriceExecution();
                    StartMenu();
                    break;
                case "8":
                    Console.Clear();
                    ReadPriceFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
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
            Console.WriteLine("Enter the name of Goods: ");
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
            Console.WriteLine("The existing positions are: \n");
            try
            {
                foreach (var item in goodsRepository.GetGoods())
                {
                    Console.WriteLine($"{item} \t");
                }
                Console.WriteLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
        }

        //Запуск работы c Units
        private void StartUnitsExecution()
        {
            bool exist = true;
            Console.WriteLine("Enter the name of Units: ");
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
            Console.WriteLine("The existing positions are: \n");
            try
            {
                foreach (var item in unitsRepository.GetUnits())
                {
                    Console.WriteLine($"{item} \t");
                }
                Console.WriteLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }

        }
        private void StartSalesExecution()
        {
            var goods = PickGoodsById();
            var units = PickUnitsById();


            var price = priceRepository.SearchPriceByMatch(goods, units);
            if (price == null)
            {
                Console.Clear();
                Console.WriteLine($"There's no price set for {units.Name} {goods.Name}.");
                Console.Write("Please, enter the price: ");
                decimal total = EnterDecimal();
                price = priceRepository.CreatePrice(goods, units, total);
            }


            Console.WriteLine("Enter quantity:");
            int quantity = EnterInt();//Done: Сделать функцию на проверку на буквы, null etc.


            var sales = salesRepository.CreateSales(goods, units, price, quantity);
            if (sales != null)
            {
                Console.WriteLine($"\nSale created successfully as id: {sales.SalesId}!\n Press Enter to continue...");
            }
            else 
                Console.WriteLine("Something went wrong...");
            Console.ReadLine();
            Console.Clear();
        }
        private void ReadSalesFile()
        {
            Console.WriteLine("Sales database: \n");
            try
            {
                foreach (var item in salesRepository.GetSales())
                {
                    int id = item.SalesGoodsId;
                    var goods = goodsRepository.SearchGoodsByID(id);
                    id = item.SalesUnitsId;
                    var units = unitsRepository.SearchUnitsByID(id);

                    //Проверка чтения
                    Console.Write($"{item.SalesId}.");
                    Console.Write($"   ");
                    if (item.SalesQuantity > 1)
                    {
                        Console.Write($"{item.SalesQuantity} {units.Name} {goods.Name}s for {item.SalesPrice}$");
                    }
                    else
                        Console.Write($"{item.SalesQuantity} {units.Name} {goods.Name} for {item.SalesPrice}$");
                    Console.WriteLine();
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
        private void StartPriceExecution()
        {
            var goods = PickGoodsById();
            var units = PickUnitsById();

            Console.Write($"The total price of {units.Name} {goods.Name}: ");
            decimal total = EnterDecimal();
            var price = priceRepository.CreatePrice(goods, units, total);
            if (price != null)
            {
                Console.WriteLine("\nPrice set successfully!\n Press Enter to continue...");
            }
            else
                Console.WriteLine("Something went wrong...");
            Console.ReadLine();
            Console.Clear();
        }
        private void ReadPriceFile()
        {
            Console.WriteLine("The existing positions are: \n");
            try
            {
                foreach (var item in priceRepository.GetPrices())
                {
                    int id = item.PriceGoodsId;
                    var goods = goodsRepository.SearchGoodsByID(id);
                    id = item.PriceUnitsId;
                    var units = unitsRepository.SearchUnitsByID(id);
                    //Проверка чтения
                    Console.Write($"{item.PriceId}.");
                    Console.Write($"   ");
                    Console.Write($"{units.Name} {goods.Name} is {item.PriceTotal}$");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
        }

        private string EnterName()
        {
            //TODO: сделать проверку на null (while). Чтобы не было пробелов. Протестить
            //Console.Write(" - ");
            string? name = Console.ReadLine();
            while (string.IsNullOrEmpty(name?.Trim()))
            {
                ClearLine();
                Console.Write(" - ");
                name = Console.ReadLine();
            }
            return name;
        }

        private Goods PickGoodsById()
        {
            Console.Clear();
            Console.WriteLine("Pick the product from database.\n");
            ReadGoodsFile();
            Console.Write("Enter the id: ");
            int id = EnterInt();
            Goods goods = goodsRepository.SearchGoodsByID(id);
            while (goods == null)
            {
                ClearLine();
                Console.WriteLine($"The product with id {id} doesn't exist in the database. Try again.\n");
                Console.Write("Enter the id: ");
                id = EnterInt();
                goods = goodsRepository.SearchGoodsByID(id);
            }
            return goods;
        }

        private Units PickUnitsById()
        {
            Console.Clear();
            Console.WriteLine($"Pick the condition of product from database.\n");
            ReadUnitsFile();
            Console.Write("Enter the id: ");
            int id = EnterInt();
            var units = unitsRepository.SearchUnitsByID(id);
            while (units == null)
            {
                ClearLine();
                Console.WriteLine($"The type of product with id {id} doesn't exist in the database. Try again.\n");
                Console.Write("Enter the id: ");
                id = EnterInt();
                units = unitsRepository.SearchUnitsByID(id);
            }
            return units;
        }
        private int EnterInt()
        {
            //Console.Write(" - ");
            int i = 0;
            while (!int.TryParse(Console.ReadLine(), out i) || int.Equals(i, 0) || i == null)
            {
                ClearLine();
                Console.Write("This is not valid input. Please try again: ");
            }
            return i;
        }
        private decimal EnterDecimal()
        {
            //Console.Write(" - ");
            decimal i = 0;
            while (!decimal.TryParse(Console.ReadLine(), out i) || i == null)
            {
                ClearLine();
                Console.Write("This is not valid input. Please try again: ");
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
