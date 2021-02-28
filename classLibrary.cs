using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInventorySystemProject
{
    public class Item
    {
        //general Info
        public string itemName;
        public string description;
        public byte itemStackLimit;
        public byte stackValue;

        //for consumables mainly
        public byte hungerBoost;
        public byte luckValue;

        //values for tools mainly
        public short itemDurabilityLimit;
        public short durabilityValue;
        public byte armorPoints;
        public byte damagePoints;

        //if usable in a specific field
        public bool wearable;
        public bool tool;
        public bool food;

        public Item(string nameOfItem, string detailsOfItem, byte stackLim, byte amountLoaded)
        {
            itemName = nameOfItem;
            description = detailsOfItem;

            itemStackLimit = stackLim;
            stackValue = amountLoaded;


        }
        public void makeIntoDamageOrDefenseEquipment(short valOfDur, short toolValue)
        {
            if (itemStackLimit == 1)
            {
                tool = true;
                wearable = false;

                durabilityValue = valOfDur;
                itemDurabilityLimit = valOfDur;
                //takes the modulus function value
                byte absoluteValOfTool =(byte) Math.Pow(Math.Pow(toolValue, 2), 0.5);
                if (toolValue < 0)
                {
                    armorPoints = absoluteValOfTool;
                }
                else if (toolValue > 0)
                {
                    damagePoints = absoluteValOfTool;
                }

            }

        }

        public void turnIntoFood(byte regeneratingValue)
        {
            tool = false;
            wearable = false;
            hungerBoost = regeneratingValue;
            luckValue = 0;
        }
        public void turnIntoLuckItem(byte minVal, byte maxVal)
        {
            tool = false;
            wearable = false;

            Random coinFlipForLuck = new Random();
            luckValue = (byte)coinFlipForLuck.Next(minVal, maxVal);
            hungerBoost = 0;

        }
        public void turnIntoArmour(short valOfDur, byte armourVal)
        {
            if (itemStackLimit == 1)
            {
                durabilityValue = valOfDur;
                itemDurabilityLimit = valOfDur;

                armorPoints = armourVal;

                tool = false;
                food = false;

                if (armorPoints >= 0 && damagePoints == 0)
                {
                    wearable = true;
                }

            }

        }
        public Item bettingItem(List<Item> possibleItems, byte howMuchRequiredMinimum)
        {
            if (itemStackLimit >= howMuchRequiredMinimum)
            {
                Random choosingWhatItemWillBeGiven = new Random();
                byte ourRandomIndexChosen = (byte)(choosingWhatItemWillBeGiven.Next(0, possibleItems.Count));
                Item newReceivedItem = new Item(possibleItems[ourRandomIndexChosen].itemName, possibleItems[ourRandomIndexChosen].description, possibleItems[ourRandomIndexChosen].itemStackLimit, possibleItems[ourRandomIndexChosen].stackValue);

                itemStackLimit -= howMuchRequiredMinimum;

                return newReceivedItem;
            }
            return new Item(null, null, 0, 0);
        }

        public byte updateItemSlot()
        {
            byte timesWeNeedMoreInstances = 0;
            if (durabilityValue <= 0)
            {
                stackValue--;
                durabilityValue = itemDurabilityLimit;
            }
            if (stackValue == 0)
            {
                itemName = null;
                description = null;
                stackValue = 0;
                itemStackLimit = 0;
            }
            
            while (stackValue > itemStackLimit)
            {
                stackValue -= itemStackLimit;
                timesWeNeedMoreInstances++;

            }
            return timesWeNeedMoreInstances;
        }

        public bool isThisSlotEmpty()
        {
            if (itemName == null || stackValue == 0)
            {
                return true;
            }
            return false;
        }
        public void turnIntoNothing()
        {
            itemName = null;
            updateItemSlot();

        }
        public Item makeCopy(Item sample)
        {
            return sample;
        }
    }
    //using minecraft inventory set up as a inspiration
    public class Inventory
    {
        public Item[][] storageOfItems;

        public Item[] armorInventory;

        public Item[] quickAccessInventory;

        public Item offHand;

        public Item mainHand;

        public byte hungerBarSize;

        public byte hungerPoints;

        public byte healthBarSize;

        public byte healthPoints;

        public byte armorBar;

        public byte armorPoints;

        public List<string> potionEffects;


        public Inventory(short xSizeOfPlayerStorage, short ySizeOfPlayerStorage, short armorSlotSize, short sizeOfQuickAccesInventory, byte statsSet)
        {
            storageOfItems = new Item[ySizeOfPlayerStorage][];
            for (byte cycleYStorage = 0; cycleYStorage < ySizeOfPlayerStorage; cycleYStorage++)
            {
                storageOfItems[cycleYStorage] = new Item[xSizeOfPlayerStorage];
                for (byte cycleXStorage = 0; cycleXStorage < xSizeOfPlayerStorage; cycleXStorage++)
                {
                    storageOfItems[cycleYStorage][cycleXStorage] = new Item(null, null, 0, 0);
                }
            }

            healthBarSize = statsSet;
            healthPoints = statsSet;

            hungerBarSize = statsSet;
            hungerPoints = statsSet;

            armorBar = statsSet;
            armorPoints = 0;

            armorInventory = new Item[armorSlotSize];
            for (byte cycleArmor = 0; cycleArmor < armorSlotSize; cycleArmor++)
            {
                armorInventory[cycleArmor] = new Item(null, null, 1, 0);
            }
            offHand = new Item(null, null, 0, 0);

            quickAccessInventory = new Item[sizeOfQuickAccesInventory];
            for (byte cycleQuickAccess = 0; cycleQuickAccess < sizeOfQuickAccesInventory; cycleQuickAccess++)
            {
                quickAccessInventory[cycleQuickAccess] = new Item(null, null, 0, 0);
            }
            mainHand = quickAccessInventory[0];

        }
        public void switchItemInBothHands()
        {
            Item temporaryStorage = mainHand;
            offHand = temporaryStorage;
            temporaryStorage = offHand;
            mainHand = offHand;

        }
        public void updateWholeInventory()
        {
            mainHand.updateItemSlot(); byte timesReAddtoMainHand = mainHand.updateItemSlot();

            Item stableMainHand = mainHand; stableMainHand.stackValue = stableMainHand.itemStackLimit;


            addNewItemOntoInventory(stableMainHand, timesReAddtoMainHand);

            offHand.updateItemSlot(); byte timesReAddtoOffHand = offHand.updateItemSlot();

            Item stableOffHand = offHand; stableOffHand.stackValue = stableOffHand.itemStackLimit;


            addNewItemOntoInventory(stableOffHand, timesReAddtoOffHand);


            for (byte cycleQuickAccess = 0; cycleQuickAccess < quickAccessInventory.Length; cycleQuickAccess++)
            {               
                quickAccessInventory[cycleQuickAccess].updateItemSlot(); byte reAddValue = quickAccessInventory[cycleQuickAccess].updateItemSlot();

                Item stableQuickAccessItem = quickAccessInventory[cycleQuickAccess]; stableQuickAccessItem.stackValue = stableQuickAccessItem.itemStackLimit;


                addNewItemOntoInventory(stableQuickAccessItem, reAddValue);
            }
            for (byte cycleArmor = 0; cycleArmor < armorInventory.Length; cycleArmor++)
            {
                armorInventory[cycleArmor].updateItemSlot(); byte reAddValue = armorInventory[cycleArmor].updateItemSlot();

                Item stableArmorItemBundle = armorInventory[cycleArmor]; stableArmorItemBundle.stackValue = stableArmorItemBundle.itemStackLimit;


                addNewItemOntoInventory(stableArmorItemBundle, reAddValue);

            }

            for (byte cycleYStorage = 0; cycleYStorage < storageOfItems.Length; cycleYStorage++)
            {
                for (byte cycleXStorage = 0; cycleXStorage < storageOfItems[0].Length; cycleXStorage++)
                {

                    storageOfItems[cycleYStorage][cycleXStorage].updateItemSlot(); byte reAddValue = storageOfItems[cycleYStorage][cycleXStorage].updateItemSlot();

                    Item stableStoredItem = storageOfItems[cycleYStorage][cycleXStorage]; stableStoredItem.stackValue = stableStoredItem.itemStackLimit;


                    addNewItemOntoInventory(stableStoredItem, reAddValue);

                }
            }
        }
        public void addNewItemOntoInventory(Item newItemGiven, byte timesWeAdd)
        {
            for(byte cycle=0; cycle<timesWeAdd; cycle++)
            {
                if (newItemGiven.stackValue <= newItemGiven.itemStackLimit)
                {
                    bool spaceFound = false;
                    if (mainHand.isThisSlotEmpty() == true)
                    {
                        Console.WriteLine("found empty slot and added "+newItemGiven.itemName);
                        mainHand = newItemGiven;
                        return;
                    }


                    for (byte cycleQuickAccess = 0; cycleQuickAccess < quickAccessInventory.Length; cycleQuickAccess++)
                    {
                        if (quickAccessInventory[cycleQuickAccess].isThisSlotEmpty() == true)
                        {
                            quickAccessInventory[cycleQuickAccess] = newItemGiven;
                            break;
                        }
                    }




                    for (byte cycleYStorage = 0; cycleYStorage < storageOfItems.Length && spaceFound==false; cycleYStorage++)
                    {
                        for (byte cycleXStorage = 0; cycleXStorage < storageOfItems[0].Length; cycleXStorage++)
                        {
                            if (storageOfItems[cycleYStorage][cycleXStorage].isThisSlotEmpty() == true)
                            {
                                storageOfItems[cycleYStorage][cycleXStorage] = newItemGiven;
                                spaceFound = true;
                                break;
                                
                                
                            }

                        }
                    }
                }
            }
            

        }
        public void displayArmorInventory(string spacingOfStats)
        {
            foreach (Item cycleOfArmor in armorInventory)
            {

                Console.WriteLine("Defense points: " + cycleOfArmor.armorPoints + spacingOfStats + "Name: " + cycleOfArmor.itemName + spacingOfStats + "Durability: " + cycleOfArmor.durabilityValue + " / " + cycleOfArmor.itemDurabilityLimit + "\n");
            }
        }

        public void select(byte positionInQuickAccess)
        {
            while (positionInQuickAccess > quickAccessInventory.Length)
            {
                positionInQuickAccess -= (byte)quickAccessInventory.Length;
            }
            Item temporaryCopyOfMianHandItem = mainHand;
            mainHand = temporaryCopyOfMianHandItem;
            quickAccessInventory[positionInQuickAccess] = temporaryCopyOfMianHandItem;

        }
        public void useConsumableAsFood()
        {
            if (mainHand.tool == false && mainHand.wearable == false && mainHand.luckValue==0)
            {
                if (hungerPoints < hungerBarSize)
                {
                    hungerPoints += mainHand.hungerBoost;
                    if (hungerPoints > hungerBarSize)
                    {
                        hungerPoints = hungerBarSize;
                    }
                    mainHand.stackValue--;
                    mainHand.updateItemSlot();
                }
            }
        }
        public void useConsumableLuckyItem(List<Item> ourListOfDropChancesFromCoinFlip)
        {
            if(mainHand.tool == false && mainHand.wearable==false && mainHand.luckValue > 0)
            {
                for(byte cycleOfRandomCoinFlip = 0; cycleOfRandomCoinFlip< mainHand.luckValue; cycleOfRandomCoinFlip++)
                {
                    //===========================================================================default amount of lucky items, you can change if you want===========================================================================\\
                    if (ourListOfDropChancesFromCoinFlip.Count == 0)
                    {
                        Random randomizerForDrops = new Random();

                        ourListOfDropChancesFromCoinFlip.Add(new Item("Dirt", "Regular Dirt", (byte)randomizerForDrops.Next(0, 255), 1));

                        ourListOfDropChancesFromCoinFlip.Add(new Item("Iron Sword", "A sword made of iron", (byte)randomizerForDrops.Next(0, 3), 1));
                        ourListOfDropChancesFromCoinFlip[1].makeIntoDamageOrDefenseEquipment(268, 7);

                        ourListOfDropChancesFromCoinFlip.Add(new Item("Rain Coat", "A simple coat meant to protect you from rain", (byte)randomizerForDrops.Next(0, 1), 1));
                        ourListOfDropChancesFromCoinFlip[2].turnIntoArmour(67, 3);

                        ourListOfDropChancesFromCoinFlip.Add(new Item("Lucky Dice", "A dice which gives you values of 1>=x>=3", (byte)randomizerForDrops.Next(0, 2), 1));
                        ourListOfDropChancesFromCoinFlip[3].turnIntoLuckItem(1, 4);
                    }
                    
                   
                    //===========================================================================default amount of lucky items, you can change if you want===========================================================================\\

                    Item ourRandomItem = mainHand.bettingItem(ourListOfDropChancesFromCoinFlip, mainHand.luckValue);
                    addNewItemOntoInventory(ourRandomItem, 1);
                }
                mainHand.stackValue--;
                mainHand.updateItemSlot();
            }
        }
        public void attemptingToWear()
        {
            
            if (mainHand.wearable == true)
            {
                for (byte cycleArmor = 0; cycleArmor < armorInventory.Length; cycleArmor++)
                {
                    if (armorInventory[cycleArmor].isThisSlotEmpty() == true)
                    {
                        armorInventory[cycleArmor] = mainHand;

                        Console.WriteLine(armorInventory[cycleArmor].itemName + " exists");
                        mainHand.stackValue--;
                        

                        mainHand.updateItemSlot();
                        Console.WriteLine(armorInventory[cycleArmor].itemName + " exists");
                        break;
                    }
                }
            }
        }
    }
}
