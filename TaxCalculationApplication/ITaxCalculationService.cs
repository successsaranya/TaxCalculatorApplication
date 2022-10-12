using System;
using System.Collections.Generic;

namespace TaxCalculationApplication
{
    public interface ITaxCalculationService
    {
        ShopBasket createShoppingObject(string item, int qty, decimal price);
        decimal calculateSalesTax(string item, bool imported, decimal price);
        decimal calculateImportTax(bool imported, decimal price);
        bool checkImportItem(string itemName);
        void generateDisplayOutput(List<ShopBasket> basketList);
    }
}
