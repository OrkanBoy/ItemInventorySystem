using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;


namespace CSharpInventorySystemProject
{

    public static class usefulTools
    {

        public static void enchantmentTransferFromBookTo(this Item thisEnchantmentBook, Item subjectOfEnchant)
        {
            if(thisEnchantmentBook.IDofItem == 5)
            {
                for (short enchantmentCycle = 0; enchantmentCycle < thisEnchantmentBook.enchantmentsOfItem.Count; enchantmentCycle++)
                {
                    thisEnchantmentBook.enchantmentsOfItem[enchantmentCycle].applyEnchant(thisEnchantmentBook, subjectOfEnchant);
                }
            }
            
        }

        public static string showOpposingEffect(this string effectTypeSubj)
        {

            switch (effectTypeSubj)
            {
                case "agility": return "slowness";
                case "slowness": return "agility";
                case "resistance": return "fragility";
                case "fragility": return "resistance";
                case "mighty": return "weakness";
                case "weakness": return "mighty";

            }
            return "empty";
        }
        

        public static List<Enchantments> fuseEnchantments(this List<string> enchantmentsGiven)
        {
            List<Enchantments> ourEnchantedResult = new List<Enchantments>();
            short[] multiplierList = new short[enchantmentsGiven.Count];

            for (byte quickCyc = 0; quickCyc < multiplierList.Length; quickCyc++)
            {
                multiplierList[quickCyc] = 1;
            }
            for (short mainCycle = 0; mainCycle < enchantmentsGiven.Count; mainCycle++)
            {

                for (short cycle = (byte)(mainCycle + 1); cycle < enchantmentsGiven.Count; cycle++)
                {
                    short valueStore = multiplierList[mainCycle];
                    
                    if (enchantmentsGiven[mainCycle] == enchantmentsGiven[cycle])
                    {
                        multiplierList[mainCycle] += valueStore;
                        multiplierList[cycle] -= valueStore;
                    }
                }
            }
            for(short currentMultiplierCyc = 0; currentMultiplierCyc < multiplierList.Length; currentMultiplierCyc++)
            {
                
                short stableMultiplier = 0;
                while(Math.Pow(2, stableMultiplier) <= multiplierList[currentMultiplierCyc])
                {
                    stableMultiplier++;
                }
                multiplierList[currentMultiplierCyc] = stableMultiplier;
            }
            for(short currentStableMultiplierCyc = 0; currentStableMultiplierCyc<multiplierList.Length; currentStableMultiplierCyc++)
            {
                multiplierList[currentStableMultiplierCyc] = (short)(Math.Log2(multiplierList[currentStableMultiplierCyc])+1);
            }
            for(byte cycle = 0; cycle<multiplierList.Length; cycle++)
            {
                
                ourEnchantedResult.Add(new Enchantments(enchantmentsGiven[cycle], multiplierList[cycle]));
                
            }
            byte foundVoid = 0;
            do
            {
                foundVoid = 0;
                for (byte Cycle = 0; Cycle < ourEnchantedResult.Count; Cycle++)
                {
                    if (ourEnchantedResult[Cycle].level <= 0)
                    {
                        ourEnchantedResult.RemoveAt(Cycle);
                        foundVoid++;
                    }
                }
            } while (foundVoid > 0);

            return ourEnchantedResult;
        }

        public static List<Effects> fuseEffects(this List<string> effectsGiven)
        {
            List<Effects> effectSolution = new List<Effects>();
            short[] multiplierList = new short[effectsGiven.Count];
            for (byte quickCyc = 0; quickCyc < multiplierList.Length; quickCyc++)
            {
                multiplierList[quickCyc] = 1;
            }
            for (short mainCycle = 0; mainCycle < effectsGiven.Count; mainCycle++)
            {

                for (short cycle = (byte)(mainCycle + 1); cycle < effectsGiven.Count; cycle++)
                {
                    short valueStore = multiplierList[mainCycle];
                    if (effectsGiven[mainCycle] == showOpposingEffect(effectsGiven[cycle]))
                    {
                        //Console.WriteLine(ourListOfEffectGiven[mainCycle] + "    " + valueStore);
                        multiplierList[mainCycle] -= valueStore;
                        //Console.WriteLine(ourListOfEffectGiven[cycle] + "    " + multiplierList[cycle]);
                        multiplierList[cycle] -= valueStore;

                    }
                    if (effectsGiven[mainCycle] == effectsGiven[cycle])
                    {
                        multiplierList[mainCycle] += valueStore;
                        multiplierList[cycle] -= valueStore;
                    }
                }
            }

            for (byte cyc = 0; cyc < effectsGiven.Count; cyc++)
            {


                string currentCycle = effectsGiven[cyc];


                string[] arrayOfBaseEffectTypes = new string[3];
                arrayOfBaseEffectTypes[0] = "baseSpeed";
                arrayOfBaseEffectTypes[1] = "baseHealth";
                arrayOfBaseEffectTypes[2] = "baseStrength";
                switch (currentCycle.ToLower())
                {
                    case "agility":
                        effectSolution.Add(new Effects(multiplierList[cyc], arrayOfBaseEffectTypes[0])); break;
                    case "slowness":
                        effectSolution.Add(new Effects(-1 * multiplierList[cyc], arrayOfBaseEffectTypes[0])); break;
                    case "resistance":
                        effectSolution.Add(new Effects(multiplierList[cyc], arrayOfBaseEffectTypes[1])); break;
                    case "fragility":
                        effectSolution.Add(new Effects(-1 * multiplierList[cyc], arrayOfBaseEffectTypes[1])); break;
                    case "mighty":
                        effectSolution.Add(new Effects(multiplierList[cyc], arrayOfBaseEffectTypes[2])); break;
                    case "weakness":
                        effectSolution.Add(new Effects(-1 * multiplierList[cyc], arrayOfBaseEffectTypes[2])); break;
                    default:
                        Random randomizer = new Random();
                        short chosenSide = (short)randomizer.Next(-1, 2);
                        while (chosenSide == 0)
                        {
                            chosenSide = (short)randomizer.Next(-1, 2);
                        }
                        effectSolution.Add(new Effects(chosenSide * multiplierList[cyc], arrayOfBaseEffectTypes[randomizer.Next(0, 3)])); break;


                }
                effectSolution[cyc].updateName();

            }

            byte foundVoid = 0;

            do
            {

                foundVoid = 0;
                for (byte Cycle = 0; Cycle < effectSolution.Count; Cycle++)
                {
                    if (effectSolution[Cycle].findEffectTytpe == "empty")
                    {
                        effectSolution.RemoveAt(Cycle);
                        foundVoid++;
                    }
                }
            } while (foundVoid > 0);
            return effectSolution;

        }
        public static void manageEnchantments(this Enchantments enchantmentWeWantToTransfer,Item enchantmentGiver, Item subjectItem)
        {
            var ourEnchantingHost = subjectItem.enchantmentsOfItem;
            var ourEnchantingDonator = enchantmentGiver.enchantmentsOfItem;
            if ((subjectItem.IDofItem == 3 || subjectItem.IDofItem == 4) && enchantmentGiver.IDofItem==5)
            {
                
                ourEnchantingHost.Add(enchantmentWeWantToTransfer);
                ourEnchantingDonator.RemoveAt(ourEnchantingDonator.IndexOf(enchantmentWeWantToTransfer));
                
            }

        }
        public static short closestBase10(short ourValue)
        {
            if (ourValue < 0)
            {
                ourValue = (short)Math.Pow(Math.Pow(ourValue, 2), 0.5);
            }
            byte startingPower = 0;
            while (ourValue > Math.Pow(10, startingPower))
            {
                startingPower++;

            }
            return (short)Math.Pow(10, startingPower);
        }
        public static void displayEfcs(this Item ourSubject, string spacing = "   ")
        {
            var effectsOfOurSubject = ourSubject.lingeringEffects;
            string accumulatedInfo = "";
            if (effectsOfOurSubject.Count > 0)
            {
                foreach (Effects currentCyc in effectsOfOurSubject)
                {
                    accumulatedInfo += currentCyc.findEffectTytpe + spacing + "speed value: " + currentCyc.baseEffectSpeed + spacing + "health value: " + currentCyc.baseEffectHealth + spacing + "strength value: " + currentCyc.baseEffectStrength + "\n";
                }
            }
            else
            {
                accumulatedInfo = "No effects";
            }

            if (ourSubject.IDofItem == 2)
            {
                Console.WriteLine("Potency Factor: " + ourSubject.potionInfo.potency + spacing + "Duration Period: " + ourSubject.potionInfo.duration + "\n" + accumulatedInfo);
            }
            else if (ourSubject.IDofItem == 1)
            {
                Console.WriteLine("Hunger Cancellation: " + ourSubject.foodStats.hungerBoost + spacing + "Required Period to consume: " + ourSubject.foodStats.timeToEat + "\n" + accumulatedInfo + "in food, duration of effects dure very short time");
            }
            else
            {
                Console.WriteLine("Effects which are not of food/potion class are offensive: " + accumulatedInfo);
            }
        }
        public static void displayEnchs(this Item ourSubject, string spacing = "   ")
        {
            var enchantmentsOfSubject = ourSubject.enchantmentsOfItem;

            string accumulatedInfo = "";
            try
            {
                foreach (Enchantments currentCyc in enchantmentsOfSubject)
                {
                    accumulatedInfo += currentCyc.enchantNameSearch + spacing + currentCyc.level+ "\n";
                }
            }
            catch(Exception)
            {
                accumulatedInfo = "No enchantments";
            }
            if(ourSubject.IDofItem == 3)
            {
                var quickPointer = ourSubject.weaponStats;
                Console.WriteLine("Attack Value: "+quickPointer.damagePoints+spacing+"Blocking ability: "+quickPointer.blockChance+spacing+"Reach of weapon: "+quickPointer.reachOfWeapon+"\n"+accumulatedInfo);
            }
            if(ourSubject.IDofItem == 4)
            {
                var quickPointer = ourSubject.clothingInfo;
                Console.WriteLine("Type of clothing/armor/dress: " + quickPointer.setTypeofClothing + spacing + "Armor points: " + quickPointer.armorPoints + spacing + "Weight of armor(unknown units): " + spacing + quickPointer.weightOfCloth + "\n" + accumulatedInfo);
            }
            if (ourSubject.IDofItem == 5)
            {
                Console.WriteLine("Author of Book: " + ourSubject.bookDetails.authorOfBook + spacing + "Contents: " + ourSubject.bookDetails.contentsOfBook + "\n" + accumulatedInfo);
            }
        }

        public struct toolDetails
        {
            public short durabilityLimit;
            public short currentDurability;
            //temporary storage for enchantments, they are not yet applied

            public toolDetails(short durabilityLimAssigned, short naturalDurability = 0)
            {
                currentDurability = naturalDurability;
                durabilityLimit = durabilityLimAssigned;
                if (naturalDurability == 0)
                {
                    currentDurability = durabilityLimAssigned;
                }

            }

        }
        public class Enchantments
        {
            private string enchantType;
            public short level;


            public Enchantments(string nameOfEnchant = "Unbreaking", short levelOfEnchant = 1)
            {
                level = levelOfEnchant;
                //default enchantmnet name is Unbreaking
                enchantType = "unbreaking";
                enchantNameSearch = nameOfEnchant;
            }
            public string enchantNameSearch
            {
                get { return enchantType; }
                set
                {
                    string[] legalEnchantments = { "unbreaking", "light weight", "protection", "sharpness", "mending", "sweeping edge","blocking power", "fire aspect"};
                    foreach (string cycleCheck in legalEnchantments)
                    {
                        if (cycleCheck == value.ToLower())
                        {
                            enchantType = value;
                            break;
                        }
                        else
                        {
                            Random randomEnchantAssigment = new Random();

                            enchantType = legalEnchantments[randomEnchantAssigment.Next(0, legalEnchantments.Length)];
                        }
                    }
                }
            }

            public void applyEnchant(Item enchantmentBook,Item subjectOfEnchant)
            {

                //ID 4 for cllothes, ID 3 for utensils
                if (subjectOfEnchant.IDofItem == 3 || subjectOfEnchant.IDofItem == 4)
                {
                    if (enchantNameSearch == "unbreaking")
                    {
                        var ourEnchantingHost = subjectOfEnchant.specificDetailsOfItem;
                        ourEnchantingHost.durabilityLimit *= (short)(level + 1);
                        ourEnchantingHost.currentDurability *= (short)(level + 1);
                        manageEnchantments(this,enchantmentBook, subjectOfEnchant);
                    }
                    if(enchantNameSearch == "mending")
                    {
                        manageEnchantments(this, enchantmentBook, subjectOfEnchant);
                    }

                }
                if (subjectOfEnchant.IDofItem == 4)
                {
                    if (enchantNameSearch == "protection")
                    {
                        var ourEnchantingHost = subjectOfEnchant.clothingInfo.armorPoints;
                        ourEnchantingHost += (float)(level * ourEnchantingHost * 0.2);
                        manageEnchantments(this,enchantmentBook, subjectOfEnchant);
                    }
                    if (enchantNameSearch == "light weight")
                    {
                        var ourEnchantingHost = subjectOfEnchant.clothingInfo.weightOfCloth;
                        ourEnchantingHost += (float)(level * ourEnchantingHost * 0.2);
                        manageEnchantments(this,enchantmentBook, subjectOfEnchant);
                    }
                }
                if (subjectOfEnchant.IDofItem == 3)
                {
                    if (enchantNameSearch == "sweeping edge")
                    {
                        var ourEnchantingHost = subjectOfEnchant.weaponStats.reachOfWeapon;
                        ourEnchantingHost += (float)(level * ourEnchantingHost * 0.1);
                        manageEnchantments(this,enchantmentBook, subjectOfEnchant);
                    }
                    if(enchantNameSearch == "sharpness")
                    {
                        Console.WriteLine("You have");
                        var ourEnchantingHost = subjectOfEnchant.weaponStats.damagePoints;

                        ourEnchantingHost += (float)(level * ourEnchantingHost * 0.1);
                        ourEnchantingHost = 1000;
                        manageEnchantments(this, enchantmentBook, subjectOfEnchant);
                    }
                    if(enchantNameSearch == "blocking power")
                    {
                        var ourEnchantingHost = subjectOfEnchant.weaponStats.blockChance;
                        ourEnchantingHost += (float)(level * ourEnchantingHost * 0.1);
                        manageEnchantments(this, enchantmentBook, subjectOfEnchant);
                    }
                    if(enchantNameSearch == "fire aspect")
                    {
                        List<string> fireEffectAccumulator = new List<string>();
                        for(short cycle = 0; cycle < level; cycle++)
                        {
                            fireEffectAccumulator.Add("fragility");
                        }
                        fuseEffects(fireEffectAccumulator);
                        manageEnchantments(this, enchantmentBook, subjectOfEnchant);
                    }
                }
            }
            

        }


        public struct weaponSpecialization
        {

            public float damagePoints;
            public float reachOfWeapon;
            public float blockChance;

            public weaponSpecialization(short valueOfWeapon, short weaponRange = 1)
            {
                byte absoluteValue = (byte)Math.Pow(Math.Pow(valueOfWeapon, 2), 0.5);
                damagePoints = 0;
                blockChance = 0;
                if (valueOfWeapon < 0)
                {
                    damagePoints = absoluteValue;
                }
                if (valueOfWeapon > 0)
                {
                    blockChance = absoluteValue;
                }
                reachOfWeapon = weaponRange;
            }
        }
        public struct foodNutritionalValue
        {
            public byte timeToEat;
            public byte hungerBoost;

            public foodNutritionalValue(byte hungerPointsIncreaser, byte consumingTime)
            {
                timeToEat = consumingTime;
                hungerBoost = hungerPointsIncreaser;

            }

        }
        public struct clothingValue
        {
            public float armorPoints;
            /*hat, helmet by default have coveredArea value of 5
             pants, trousres have default coveredArea value of 8
            socks and shoes have default coveredArea value of 4
            cloth, chestplate have default coveredArea value of 9



             */


            public float weightOfCloth;
            private string typeOfClothing;

            public clothingValue(short armorValueGiven, short weightAssigned = 0, string desiredClothType = "cloth")
            {

                armorPoints = armorValueGiven;

                if (weightAssigned == 0)
                {
                    //default way of balancing armor/clothing, more armorPoints, more WieighTvalue
                    weightAssigned = (short)((float)armorValueGiven / (float)closestBase10(armorValueGiven));
                }
                weightOfCloth = weightAssigned;

                typeOfClothing = desiredClothType;


            }
            public string setTypeofClothing
            {
                get { return typeOfClothing; }
                set
                {
                    List<string> allowedTypesOfClothing = new List<string>() { "hat", "helmet", "pants", "trousers", "shoes", "socks", "chestplate" };
                    foreach (string cycleOfSelection in allowedTypesOfClothing)
                    {
                        if (value.ToLower() == cycleOfSelection)
                        {
                            typeOfClothing = value;
                            break;
                        }
                        else { typeOfClothing = "cloth"; }
                    }
                }
            }

        }
        public struct potionValue
        {
            public byte potency;
            public short duration;




            public potionValue(byte potencyGiven = 1, short durationGiven = 60)
            {
                potency = potencyGiven;
                duration = durationGiven;

            }






        }

        public struct Effects
        {

            /*public byte rateOfEffectPerUnitDuration
                          ^^^^^^^ I added this just incase i want to add new effect types or make a change the whole potion system
            */
            //doesnt matter the magnitude, we only care of if its either {+,0,-}

            public short baseEffectStrength;
            public short baseEffectHealth;
            public short baseEffectSpeed;

            private string effectName;

            public Effects(int effectValGiven = 0, string baseType = "baseEmpty")
            {
                baseEffectStrength = 0;
                baseEffectHealth = 0;
                baseEffectSpeed = 0;

                if (baseType.ToLower() == "basestrength")
                {
                    baseEffectStrength = (short)effectValGiven;
                }
                else if (baseType.ToLower() == "basehealth")
                {
                    baseEffectHealth = (short)effectValGiven;
                }
                else if (baseType.ToLower() == "basespeed")
                {
                    baseEffectSpeed = (short)effectValGiven;
                }
                effectName = "empty";
                findEffectTytpe = "";
            }
            public string findEffectTytpe
            {
                get { return effectName; }
                set
                {
                    if (baseEffectSpeed > 0 && baseEffectHealth == 0 && baseEffectStrength == 0)
                    {
                        effectName = "agility";
                    }
                    else if (baseEffectSpeed < 0 && baseEffectHealth == 0 && baseEffectStrength == 0)
                    {
                        effectName = "slowness";
                    }
                    else if (baseEffectSpeed == 0 && baseEffectHealth > 0 && baseEffectStrength == 0)
                    {
                        effectName = "resistance";
                    }
                    else if (baseEffectSpeed == 0 && baseEffectHealth < 0 && baseEffectStrength == 0)
                    {
                        effectName = "fragility";
                    }
                    else if (baseEffectSpeed == 0 && baseEffectHealth == 0 && baseEffectStrength > 0)
                    {
                        effectName = "mighty";
                    }
                    else if (baseEffectSpeed == 0 && baseEffectHealth == 0 && baseEffectStrength < 0)
                    {
                        effectName = "weakness";
                    }

                    //the following effects are place holders if the editor wants to add more effect types
                    else if (baseEffectSpeed != 0 || baseEffectHealth != 0 || baseEffectStrength != 0)
                    {
                        effectName = "other";
                    }
                    else
                    {
                        effectName = "empty";
                    }
                }
            }
            public void updateName()
            {
                findEffectTytpe = effectName;
            }

        }


        public struct bookValue
        {
            public string contentsOfBook;
            public string authorOfBook;
            public bookValue(string deliveredContents = null, string supposedAuthor="unknown")
            {
                contentsOfBook = deliveredContents;

                authorOfBook = supposedAuthor;
            }

        }


        public struct Item
        {
            //general Info
            public string itemName;
            public string description;
            public byte IDofItem;
            public byte itemStackLimit;
            public byte stackValue;



            //if ID == 1, for food
            public foodNutritionalValue foodStats;

            //if ID == 2, for potion effect
            public potionValue potionInfo;

            //If ID == 3, for weapons
            public weaponSpecialization weaponStats;

            //if ID==4, for clothing
            public clothingValue clothingInfo;

            public toolDetails specificDetailsOfItem;

            //this next List<Effects> is 100% to work with potions
            //but if we apply it to food there is a small possibility and it lasts a short random period
            public List<Effects> lingeringEffects;

            public List<Enchantments> enchantmentsOfItem;

            //for ID == 5, books
            public bookValue bookDetails;
            public Item(string nameOfItem, string detailsOfItem, byte stackLim, byte amountLoaded)
            {
                itemName = nameOfItem;
                description = detailsOfItem;
                itemStackLimit = stackLim;
                stackValue = amountLoaded;
                //default ID is 0
                IDofItem = 0;
                foodStats = new foodNutritionalValue();
                potionInfo = new potionValue();
                weaponStats = new weaponSpecialization();
                clothingInfo = new clothingValue();
                specificDetailsOfItem = new toolDetails();
                lingeringEffects = new List<Effects>();
                enchantmentsOfItem = new List<Enchantments>();
                bookDetails = new bookValue();
            }
            public void assignWeaponRole(short DfsOrDmgValue, short weaponRange, short durabilityGiven = 100)
            {
                IDofItem = 3;
                specificDetailsOfItem = new toolDetails(durabilityGiven);
                enchantmentsOfItem = new List<Enchantments>();
                weaponStats = new weaponSpecialization(DfsOrDmgValue, weaponRange);
            }
            public void assignFoodUse(byte hungerVal, byte timeRequiredToConsume)
            {
                IDofItem = 1;
                foodStats = new foodNutritionalValue(hungerVal, timeRequiredToConsume);
            }
            public void assignClothingRole(short armorValue, short weightAssigned = 0, short durabilityGiven = 100, string clothingType = "cloth")
            {
                IDofItem = 4;
                specificDetailsOfItem = new toolDetails(durabilityGiven);
                enchantmentsOfItem = new List<Enchantments>();
                clothingInfo = new clothingValue(armorValue, weightAssigned, clothingType);
                clothingInfo.setTypeofClothing = clothingType;
            }
            public void assignPotionUse(byte intensityInitiation, byte durationInitiation, List<string> effectsAssigned)
            {
                IDofItem = 2;
                potionInfo = new potionValue(intensityInitiation, durationInitiation);
                lingeringEffects = fuseEffects(effectsAssigned);
            }
            public void turnIntoBook(string contentsWanted, string authorOfBook = "unknown")
            {
                IDofItem = 5;
                bookDetails = new bookValue(contentsWanted, authorOfBook);
            }

            public void turnIntoEnchantedItem(List<string> enchantmentsWanted)
            {
                if(IDofItem==3 || IDofItem == 4 || IDofItem == 5)
                {
                    enchantmentsOfItem = fuseEnchantments(enchantmentsWanted);
                }
                

            }
        }




        //using minecraft inventory set up as a inspiration
        public class Inventory
        {
            public string playerNickName;

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

            public byte expBar;

            public List<Effects> statusEffects;


            public Inventory(short xSizeOfPlayerStorage, short ySizeOfPlayerStorage, short sizeOfQuickAccesInventory, byte statsSet)
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

                armorInventory = new Item[4];
                for (byte cycleArmor = 0; cycleArmor < armorInventory.Length; cycleArmor++)
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



        }



    }
}


