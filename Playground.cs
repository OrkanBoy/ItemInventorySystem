using System;

namespace CSharpInventorySystemProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory myInventory = new Inventory(3, 9, 4, 9, 10);
            Item newArmorPiece = new Item("Iron Helmet", "helmet made of iron", 1, 1);
            newArmorPiece.turnIntoArmour(230, 5);
            myInventory.addNewItemOntoInventory(newArmorPiece, 1);

            myInventory.attemptingToWear();
            Console.WriteLine(myInventory.armorInventory[0].itemName);
            myInventory.displayArmorInventory("    ");
            
        }
    }
}
