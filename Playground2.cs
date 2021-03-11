using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_System
{
    
    using static usefulTools;
    class Program
    {
        static void Main(string[] args)
        {
            Item myBook = new Item("English Literature Book", "A book for nerds");



            List<string> enchantmentList = new List<string>();



            myBook.turnIntoBook("\nBob: Hello!", "Bob the Builder");

            enchantmentList.Add("unbreaking"); enchantmentList.Add("unbreaking");
            enchantmentList.Add("mending"); enchantmentList.Add("unbreaking"); enchantmentList.Add("mending"); enchantmentList.Add("unbreaking");
            enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("mending"); enchantmentList.Add("unbreaking");
            myBook.updateEnchantmentStatus(enchantmentList);

            List<string> newListEnchs = new List<string>();









            myBook.displayEnchs();
        }
    }
}
