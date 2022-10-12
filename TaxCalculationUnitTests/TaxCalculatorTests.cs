using System;
using System.Collections.Generic;
using TaxCalculationApplication;
using Xunit;

namespace TaxCalculationUnitTests
{
    
    public class TaxCalculatorTests
    {
        TaxCalculatorService taxCalculatorService = new TaxCalculatorService();
        
        [Fact]
        public void Test1()
        {
            List<ShopBasket> cart1 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 1:");
            cart1.Add(taxCalculatorService.createShoppingObject("book", 1, 12.49M));
            cart1.Add(taxCalculatorService.createShoppingObject("Music CD", 1, 14.99M));
            cart1.Add(taxCalculatorService.createShoppingObject("Chocolate bar", 1, 0.85M));

            taxCalculatorService.generateDisplayOutput(cart1);
        }

        [Fact]
        public void Test2()
        {
            List<ShopBasket> cart2 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 2:");
            cart2.Add(taxCalculatorService.createShoppingObject("imported box of chocolates", 1, 10.00M));
            cart2.Add(taxCalculatorService.createShoppingObject("imported bottle of perfume", 1, 47.50M));           

            taxCalculatorService.generateDisplayOutput(cart2);

        }

        [Fact]
        public void Test3()
        {
            List<ShopBasket> cart3 = new List<ShopBasket>();
            Console.WriteLine("Shopping Basket 3:");
            cart3.Add(taxCalculatorService.createShoppingObject("imported bottle of perfume ", 1, 27.99M));
            cart3.Add(taxCalculatorService.createShoppingObject("bottle of perfume", 1, 18.99M));
            cart3.Add(taxCalculatorService.createShoppingObject("packet of headache pills", 1, 9.75M));
            cart3.Add(taxCalculatorService.createShoppingObject("imported box of chocolates", 1, 11.25M));           

            taxCalculatorService.generateDisplayOutput(cart3);

        }
    }
}
