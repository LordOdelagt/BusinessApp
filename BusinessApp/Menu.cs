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
            Console.Write(" - ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1"://Создать новый Category
                    StartCategoryExecution();
                    StartMenu();
                    break;
                case "2"://Считать Category
                    Console.Clear();
                    ReadCategoryFile();
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
            var Category = PickCategoryById();
            var units = PickUnitsById();


            var price = priceRepository.SearchPriceByMatch(Category.Id, units.Id);
            if (price == null)
            {
                Console.Clear();
                Console.WriteLine($"There's no price set for {units.Name} {Category.Name}.");
                Console.Write("Please, enter the price: ");
                decimal total = EnterDecimal();
                price = priceRepository.CreatePrice(Category, units, total);
            }


            Console.WriteLine("Enter quantity:");
            int quantity = EnterInt();//Done: Сделать функцию на проверку на буквы, null etc.


            var sales = salesRepository.CreateSales(Category, units, price, quantity);
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

                    //Проверка чтения
                    Console.Write($"{item.SalesId}.");
                    Console.Write($"   ");
                    if (item.SalesQuantity > 1)
                    {
                        Console.Write($"{item.SalesQuantity} {units.Name} {Category.Name}s for {item.SalesPrice}$");
                    }
                    else
                        Console.Write($"{item.SalesQuantity} {units.Name} {Category.Name} for {item.SalesPrice}$");
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
            var price = priceRepository.CreatePrice(Category, units, total);
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

        private Category PickCategoryById()
        {
            Console.Clear();
            Console.WriteLine("Pick the product from database.\n");
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
