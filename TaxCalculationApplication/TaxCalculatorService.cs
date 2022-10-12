using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace TaxCalculationApplication
{
    public class TaxCalculatorService
    {
        ShopBasket shopCarts;
        string[] exceptionsItems;
        private static ILogger _logger = null;

        public TaxCalculatorService()
        {
            createLogging();
            createListFromExceptions();
            shoppingBaskets();
        }

        public bool checkImportItem(string itemName)
        {
            return itemName.ToLower().Contains("imported") ? true : false;
        }

        public ShopBasket createShoppingObject(string item, int qty, decimal price)
        {
            try
            {
                shopCarts = new ShopBasket();
                shopCarts.quantity = qty;
                shopCarts.itemName = item;
                shopCarts.price = price;
                shopCarts.imported = checkImportItem(shopCarts.itemName);

                if (shopCarts.imported)
                    shopCarts.itemTax = calculateSalesTax(shopCarts.itemName, shopCarts.imported, shopCarts.price) + calculateImportTax(shopCarts.imported, shopCarts.price);
                else
                    shopCarts.itemTax = calculateSalesTax(shopCarts.itemName, shopCarts.imported, shopCarts.price);

                shopCarts.itemTotalwithTax = Math.Round((shopCarts.itemTax + shopCarts.price), 2);               
            }
            catch(Exception s)
            {
                _logger.LogError("Issue in Creating Shopping Object for Inputs :" + s.StackTrace);
            }
            return shopCarts;
        }


        public decimal calculateImportTax(bool importItem, decimal price)
        {
            decimal importTax = 0.0M;
            try
            {
                decimal tax;
                if (ConfigurationManager.AppSettings["ImportTax"] != null)
                    tax = decimal.Parse(ConfigurationManager.AppSettings["ImportTax"], CultureInfo.InvariantCulture);
                else
                    tax = 5.00M;

                decimal iTax = price * (Convert.ToDecimal(tax) / 100);

                importTax = importItem ? (Math.Ceiling(iTax * 20) / 20) : 0.0M;
            }
            catch(Exception e)
            {
                _logger.LogError("Issue in caclculating ImportTax :" + e.StackTrace);
            }

            return importTax;           
        }

        public decimal calculateSalesTax(string item, bool importItem, decimal price)
        {
            decimal salesTax = 0.0M;

            try
            {
                decimal sTax;
                if (ConfigurationManager.AppSettings["SalesTax"] != null)
                    sTax = decimal.Parse(ConfigurationManager.AppSettings["SalesTax"], CultureInfo.InvariantCulture);
                else
                    sTax = 10.00M;

                decimal stax = price * (sTax / 100);

                salesTax = exceptionsItems.Any(z => z.ToLower().Contains(item.ToLower()) || item.ToLower().Contains(z.ToLower())) ? 0.0M : (Math.Ceiling(stax * 20) / 20);
            }
            catch(Exception e)
            {
                _logger.LogError("Issue in caclculating SalesTax :" + e.StackTrace);
            }
            return salesTax;
        }

        public void createListFromExceptions()
        {
            string exceptionListstr;
            try
            {
                if (ConfigurationManager.AppSettings["ExceptionList"] != null)
                    exceptionListstr = ConfigurationManager.AppSettings["ExceptionList"];
                else
                    exceptionListstr = "book,medicine,pills,chocolate,chocolate bar";

                exceptionsItems = exceptionListstr.Split(',');
            }
            catch(Exception e)
            {
                _logger.LogError("Issue in converting the Exception Items to List :" + e.StackTrace);
            }
            
        }

        public void generateDisplayOutput(List<ShopBasket> shopList)
        {
            decimal salesTaxTotal = 0.0M;
            decimal CartTotal = 0.0M;
            salesTaxTotal = (from x in shopList select x.itemTax).Sum();
            CartTotal = shopList.Sum(x => x.itemTotalwithTax);
            shopList.ForEach(x => Console.WriteLine(x.quantity + " " + x.itemName + ": " + x.itemTotalwithTax));
            Console.WriteLine("Sales Taxes:" + salesTaxTotal);
            Console.WriteLine("Total:" + CartTotal);
        }

        public void shoppingBaskets()
        {
            List<ShopBasket> cart1 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 1:");
            cart1.Add(createShoppingObject("book", 1, 12.49M));
            cart1.Add(createShoppingObject("Music CD", 1, 14.99M));
            cart1.Add(createShoppingObject("Chocolate bar", 1, 0.85M));

            generateDisplayOutput(cart1);

            List<ShopBasket> cart2 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 2:");
            cart2.Add(createShoppingObject("imported box of chocolates", 1, 10.00M));
            cart2.Add(createShoppingObject("imported bottle of perfume", 1, 47.50M));

            generateDisplayOutput(cart2);

            List<ShopBasket> cart3 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 3:");
            cart3.Add(createShoppingObject("imported bottle of perfume ", 1, 27.99M));
            cart3.Add(createShoppingObject("bottle of perfume", 1, 18.99M));
            cart3.Add(createShoppingObject("packet of headache pills", 1, 9.75M));
            cart3.Add(createShoppingObject("imported box of chocolates", 1, 11.25M));

            generateDisplayOutput(cart3);
        }

        public void createLogging()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            _logger = logger;
        }
    }

    public class ShopBasket
    {
        public string itemName { get; set; }
        public int quantity { get; set; }
        public bool imported { get; set; }
        public decimal price { get; set; }
        public decimal itemTax { get; set; }
        public decimal itemTotalwithTax { get; set; }
    }
}
