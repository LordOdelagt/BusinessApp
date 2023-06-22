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
        ICategoryRepository categoryRepository = new CategoryRepository(new ExceptionLogToConsole());
        IGoodsRepository goodsRepository = new GoodsRepository(new ExceptionLogToConsole());
        IUnitsRepository unitsRepository = new UnitsRepository(new ExceptionLogToConsole());
        ISalesRepository salesRepository = new SalesRepository(new ExceptionLogToConsole());
        IPriceRepository priceRepository = new PriceRepository(new ExceptionLogToConsole());
        public void StartMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome! This is Business App.\n");
            Console.WriteLine("Please, choose the option:");
            Console.WriteLine("\n1. Create new Category\n2. Check Category\n3. Create new Units\n4. Check Units\n5. Create new Sales\n6. Check sales\n0. Exit");
            Navigation();
        }
        private void Navigation()
        {
            //Console.Write("- ");
            string? input = Convert.ToString(Console.ReadLine());
            switch (input)
            {
                //Работа с Goods
                case "1":
                    StartGoodsExecution();
                    StartMenu();
                    break;
                case "2":
                    Console.Clear();
                    ReadGoodsFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "g":
                    goto case "1";

                //Работа с Category
                case "3":
                    StartCategoryExecution();
                    StartMenu();
                    break;
                case "4":
                    Console.Clear();
                    ReadCategoryFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "c":
                    goto case "3";

                //Работа с Units
                case "5":
                    StartUnitsExecution();
                    StartMenu();
                    break;
                case "6":
                    Console.Clear();
                    ReadUnitsFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "u":
                    goto case "5";

                //Работа с Price
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
                case "p":
                    goto case "7";

                //Работа с Sales
                case "9":
                    StartSalesExecution();
                    StartMenu();
                    break;
                case "10":
                    Console.Clear();
                    ReadSalesFile();
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
                case "s":
                    goto case "9";


                default:
                    Console.WriteLine("There's no such option! Please try again. Press Enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    StartMenu();
                    break;
            }
        }

        private void StartGoodsExecution()
        {
            Console.WriteLine("Enter the name of new product: ");
            string? name = EnterName();
            var goods = goodsRepository.GetGoodsByName(name);
            if (goods != null)
            {
                GoodsMatchCheckTrue(name);
            }

            var category = PickCategoryById();

            var units = PickUnitsById();

            Console.WriteLine($"Enter the quantity of {units.Name} {goods.Name} to add in database: ");
            goods.GoodsQuantity = EnterInt();

            goodsRepository.CreateGoods(goods.Name, goods.CategoryId, goods.UnitsId, goods.GoodsQuantity);

            Console.WriteLine($"Position created successfully as id {goods.Id}!\n Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
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
                    var category = categoryRepository.SearchCategoryByID(item.CategoryId);
                    var units = unitsRepository.SearchUnitsByID(item.UnitsId);

                    //Проверка чтения
                    Console.Write($"{item.Id}.");
                    Console.Write($"   ");
                    Console.Write($"{item.GoodsQuantity} {units.Name} {item.Name}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the CSV file: " + e.Message);
            }
        }
        //Запуск работы c Category
        private void StartCategoryExecution()
        {
            bool exist = true;
            Console.WriteLine("Enter the name of Category: ");
            string? name = EnterName();
            var Category = categoryRepository.GetCategoryByName(name);
            if (Category == null)
            {
                categoryRepository.CreateCategory(name);
                exist = true;
            }
            if (exist)
            {
                Console.WriteLine($"Position created successfully as id {Category.Id}!\n Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                CategoryMatchCheckTrue(name);
            }
        }
        private void CategoryMatchCheckTrue(string name)
        {
            Console.WriteLine($"{name} already exists. Do you want to try again? y/n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    StartCategoryExecution();
                    break;
                case "n":
                    Console.Clear();
                    return;
            }
        }
        //Читает информацию с файла Category.
        private void ReadCategoryFile()
        {
            Console.WriteLine("The existing positions are: \n");
            try
            {
                foreach (var item in categoryRepository.GetCategory())
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
                Console.WriteLine($"Position created successfully as id {units.Id}!\n Press Enter to continue...");
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
            //var Category = PickCategoryById();
            //var units = PickUnitsById();

            var price = priceRepository.SearchPriceByMatch(goods.CategoryId, goods.UnitsId);
            if (price == null)
            {
                Console.Clear();
                Console.WriteLine($"There's no price set!");
                Console.Write("Please, set the new price: ");
                decimal total = EnterDecimal();
                price = priceRepository.CreatePrice(goods.CategoryId, goods.UnitsId, total);
            }

            Console.WriteLine("Enter quantity:");
            int quantity = EnterInt();

            //TODO: тут должна быть проверка quantity из goods
            while (quantity>goods.GoodsQuantity)
            {
                ClearLine();
                Console.WriteLine("There's not enough products for sale! Please try again!");
                Console.WriteLine("Enter quantity:");
                quantity = EnterInt();
            }

            //Обновляет количество товара на складе
            goodsRepository.Delete(goods.Id);
            goodsRepository.CreateGoods(goods.Name, goods.CategoryId, goods.UnitsId, goods.GoodsQuantity-quantity);

            var sales = salesRepository.CreateSales(goods.Id, goods.CategoryId, goods.UnitsId, price.PriceTotal, quantity);
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
                    var Category = categoryRepository.SearchCategoryByID(item.SalesCategoryId);
                    var units = unitsRepository.SearchUnitsByID(item.SalesUnitsId);
                    var goods = goodsRepository.SearchGoodsByID(item.SalesGoodsId);
                    //Проверка чтения
                    Console.Write($"{item.SalesId}.");
                    Console.Write($"   ");
                    if (item.SalesQuantity > 1)
                    {
                        Console.Write($"{item.SalesQuantity} {units.Name} {goods.Name} {Category.Name}s for {item.SalesPrice}$");
                    }
                    else
                        Console.Write($"{item.SalesQuantity} {units.Name} {goods.Name} {Category.Name} for {item.SalesPrice}$");
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
            var Category = PickCategoryById();
            var units = PickUnitsById();

            Console.Write($"The total price of {units.Name} {Category.Name}: ");
            decimal total = EnterDecimal();
            var price = priceRepository.CreatePrice(Category.Id, units.Id, total);
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
            Console.WriteLine("Price sheet: \n");
            try
            {
                foreach (var item in priceRepository.GetPrices())
                {
                    int id = item.PriceCategoryId;
                    var Category = categoryRepository.SearchCategoryByID(id);
                    id = item.PriceUnitsId;
                    var units = unitsRepository.SearchUnitsByID(id);
                    //Проверка чтения
                    Console.Write($"{item.PriceId}.");
                    Console.Write($"   ");
                    Console.Write($"{units.Name} {Category.Name} is {item.PriceTotal}$");
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

        private Category PickCategoryById()
        {
            Console.Clear();
            Console.WriteLine("Pick the Category from database.\n");
            ReadCategoryFile();
            Console.Write("Enter the id: ");
            int id = EnterInt();
            Category Category = categoryRepository.SearchCategoryByID(id);
            while (Category == null)
            {
                ClearLine();
                Console.WriteLine($"The product with id {id} doesn't exist in the database. Try again.\n");
                Console.Write("Enter the id: ");
                id = EnterInt();
                Category = categoryRepository.SearchCategoryByID(id);
            }
            return Category;
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
            while (!int.TryParse(Console.ReadLine(), out i) || i==0)
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
            while (!decimal.TryParse(Console.ReadLine(), out i))
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
