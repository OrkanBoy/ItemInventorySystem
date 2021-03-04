using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace CSharpInventorySystemProject
{
    using static usefulTools;
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine(increase(5, 2));

            Item myTool = new Item();
            myTool.assignWeaponRole(-10, 4);


            Item myBook = new Item();



            List<string> enchantmentList = new List<string>();

            

            myBook.turnIntoBook("\nBob: Hello!", "Bob the Builder");
            myBook.turnIntoEnchantedItem(enchantmentList);

            usefulTools.enchantmentTransferFromBookTo(myBook, myTool);

            

            myTool.displayEnchs();


        }
        static byte increase(byte valueX, byte valueY = 1)
        {
            return (byte)(valueX + valueY);
            
        }
        

    }

    
    
}
