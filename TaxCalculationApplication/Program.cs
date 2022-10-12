using System;
using Microsoft.Extensions.Logging;
    
namespace TaxCalculationApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TaxCalculatorService taxCalculator = new TaxCalculatorService();
            Console.ReadKey();          
        }       
    }
}
