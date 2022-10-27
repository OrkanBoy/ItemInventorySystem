# ItemInventorySystem
My goal is to make inventory system similar to minecraft's one with Terminal CUI
This task was suggested for me by a good friend who I look upto.

The Actual task is: 


Create an Item System, using the system in question, one should be able to:
-Create an Inventory of a set Size, that can store up to Size stacks of Items
-Create and store various Items that have Names, Descriptions, Amounts, and variable Stack sizes
-On demand, the user should be able to Drop or Use any given Item in their Inventory
-The player should be able to Equip certain Items, but only 1 at a time and if they Equip something else, their currently Equipped Item goes back to inventory
-When an Item is Used it should do one of three things depending on type: 
--Print out: "Your HP has been increased by 50" (Health system not necessary, printing is good enough)
--Equip an item and Print out: "You have equipped ItemName"
--Add a new random item to your inventory and print out: "You have opened ItemName and received ReceivedItemName"
Optional Objectives:
-A third party should be able to add their own new items to the registry
--Extra bonus if it can be done using a single function call.
-A third party should be able to define their own custom Use function
-The Inventory system should also be able to be applied to a Chest that can be opened, is a random size, and is filled with random Items including those defined by third parties, the user should then be able to see the both their inventory and the chest's, and choose items from either to switch.
