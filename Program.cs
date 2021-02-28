using System;

using System.Collections.Generic;

namespace LearningCSharpPart1
{
    class Program
    {
        static void Main(string[] args)
        {

            //labyrinthFiniteVersionGeneratorGame(12, 12,"  ", "{}", "XX", 2, 3, 11, 5);
            labyrinthInfiniteVersionGeneratorGame(2, 2, 1, 1, "  ", "{}", "XX", -1);
            //============================================================================================ == ===== = == ============== = ==================================== =================================== == == ========= ============ ================== = ==========


            static void labyrinthInfiniteVersionGeneratorGame(byte generatingAreaXSide, byte generatingAreaYSide, byte preparedGenerationXSide, byte preparedGenerationYSide, string fillerOfEmptyDungeonSpace, string fillerOfWallDungeonArea, string playerCurrentPositionFiller, short generationPattern)
            {

                if (generationPattern < 0)
                {
                    generationPattern = (short)(Math.Pow((float)generatingAreaXSide * (float)generatingAreaYSide, 0.5) * 2.5);

                }
                ifDFSShouldEnd DFSEndCheck = new ifDFSShouldEnd(generationPattern);
                Console.WriteLine(DFSEndCheck.generationRegularity + " non POGGERS\n\n\n\n");

                List<List<Room>> labyrinth = new List<List<Room>>();
                short positionXOfStartingPoint = generatingAreaXSide;
                short positionYOfStartingPoint = generatingAreaYSide;
                short totalXSize = (short)(generatingAreaXSide * 2 + 1);
                short totalYsize = (short)(generatingAreaYSide * 2 + 1);
                for (short labyrinthConstructionInEachRow = 0; labyrinthConstructionInEachRow < totalYsize; labyrinthConstructionInEachRow++)
                {
                    labyrinth.Add(new List<Room> { });

                    for (short labyrinthConstructionFillingEachCell = 0; labyrinthConstructionFillingEachCell < totalXSize; labyrinthConstructionFillingEachCell++)
                    {
                        //============
                        labyrinth[labyrinthConstructionInEachRow].Add(new Room(false, false, false, false, false, false));
                    }
                }

                string movementType = "null";
                while (movementType != "P" && movementType != "AI")
                {
                    Console.Write("What method do you want to use to travrse this labyrinth, Automated Breadth For Search guided(type 'AI'), or manually guided(type 'P')?");
                    movementType = Console.ReadLine();

                }


                labyrinth[positionXOfStartingPoint][positionYOfStartingPoint].playerPresenceCheck = true;




                byte gameEndedCheck = 0;
                while (gameEndedCheck != 10)
                {
                    totalXSize = (short)labyrinth[0].Count; Console.WriteLine(labyrinth[0].Count);
                    totalYsize = (short)labyrinth.Count; Console.WriteLine(labyrinth.Count);

                    short[] coordOfOurPlayerPos = coordinatesOfCurPosMarker(labyrinth);

                    List<List<short>> listOfCheckedRoomCoords = new List<List<short>>();

                    for (short cycleYVal = 0; cycleYVal < totalYsize; cycleYVal++)
                    {
                        for (short cycleXVal = 0; cycleXVal < totalXSize; cycleXVal++)
                        {
                            if (labyrinth[cycleYVal][cycleXVal].checkForDoorOpeningStability == true)
                            {
                                listOfCheckedRoomCoords.Add(new List<short> { cycleYVal, cycleXVal });
                            }
                            if (labyrinth[cycleYVal][cycleXVal].isThisRoomAllLocked() == false)
                            {
                                labyrinth[cycleYVal][cycleXVal].checkForDoorOpeningStability = true;
                            }
                        }
                    }


                    Random whichRoomWillWeStartFrom = new Random();



                    if (listOfCheckedRoomCoords.Count > 0)
                    {
                        short amountOfRoomsNotcheckedAroundMarker = 1;
                        while (amountOfRoomsNotcheckedAroundMarker > 0)
                        {
                            short roomChosenToGenerateFrom = (short)(whichRoomWillWeStartFrom.Next(0, listOfCheckedRoomCoords.Count));

                            //Console.WriteLine("chosen randomly " + listOfCheckedRoomCoords[roomChosenToGenerateFrom][1]+"    "+ listOfCheckedRoomCoords[roomChosenToGenerateFrom][0]);
                            depthFirstSearchConstructionLabyrinthBased(labyrinth, listOfCheckedRoomCoords[roomChosenToGenerateFrom][1], listOfCheckedRoomCoords[roomChosenToGenerateFrom][0], "X", true, preparedGenerationXSide, preparedGenerationYSide, coordOfOurPlayerPos[1], coordOfOurPlayerPos[0], DFSEndCheck);

                            DFSEndCheck.generationRegularity = (short)generationPattern;
                            DFSEndCheck.update();

                            //Console.WriteLine(listOfCheckedRoomCoords[roomChosenToGenerateFrom][1]+" is the x "+ listOfCheckedRoomCoords[roomChosenToGenerateFrom][0]+" is the y, we chose to start the genaration from");

                            labyrinth[listOfCheckedRoomCoords[roomChosenToGenerateFrom][0]][listOfCheckedRoomCoords[roomChosenToGenerateFrom][1]].candidateForGeneration = false;
                            listOfCheckedRoomCoords.RemoveAt(roomChosenToGenerateFrom);


                            amountOfRoomsNotcheckedAroundMarker = 0;

                            for (short cycleRow = 0; cycleRow < totalYsize; cycleRow++)
                            {
                                for (short cycleCell = 0; cycleCell < totalXSize; cycleCell++)
                                {

                                    if (absoluteVal(cycleRow - coordOfOurPlayerPos[0]) <= preparedGenerationYSide && absoluteVal(cycleCell - coordOfOurPlayerPos[1]) <= preparedGenerationXSide && labyrinth[cycleRow][cycleCell].checkForDoorOpeningStability == false)
                                    {

                                        //Console.WriteLine(cycleCell + "    and    " + cycleRow);
                                        amountOfRoomsNotcheckedAroundMarker++;
                                    }
                                    bool mayAddThisCoord = false;


                                    foreach (List<short> cycleOfExistenceCheck in listOfCheckedRoomCoords)
                                    {
                                        if ((cycleOfExistenceCheck[0] != cycleRow || cycleOfExistenceCheck[1] != cycleCell) && labyrinth[cycleRow][cycleCell].checkForDoorOpeningStability == true && labyrinth[cycleRow][cycleCell].candidateForGeneration == true)
                                        {

                                            mayAddThisCoord = true;

                                        }
                                        else
                                        {
                                            mayAddThisCoord = false;
                                            break;
                                        }

                                    }
                                    if (mayAddThisCoord == true)
                                    {




                                        listOfCheckedRoomCoords.Add(new List<short> { cycleRow, cycleCell });
                                    }


                                }

                            }







                        }

                    }
                    else
                    {
                        depthFirstSearchConstructionLabyrinthBased(labyrinth, coordOfOurPlayerPos[1], coordOfOurPlayerPos[0], "X", true, preparedGenerationXSide, preparedGenerationYSide, coordOfOurPlayerPos[1], coordOfOurPlayerPos[0], DFSEndCheck);
                    }





                    //thsi for stewmnet is supposed to cyel through each room and it cheks if the area around the amrker is checked and if the whole area is cheked then{i use the checkForDoorStability property of my instances of the Room method} the amount of unchecked rooms satys at 0

                    short amountOfRoomsUncheckedAroundMarker = 0;
                    for (short cycleRow = 0; cycleRow < totalYsize; cycleRow++)
                    {
                        for (short cycleCell = 0; cycleCell < totalXSize; cycleCell++)
                        {

                            if (absoluteVal(cycleRow - coordOfOurPlayerPos[0]) <= preparedGenerationYSide && absoluteVal(cycleCell - coordOfOurPlayerPos[1]) <= preparedGenerationXSide && labyrinth[cycleRow][cycleCell].checkForDoorOpeningStability == false)
                            {

                                Console.WriteLine(cycleCell + "    and    " + cycleRow);
                                amountOfRoomsUncheckedAroundMarker++;
                            }

                        }
                    }


                    Console.WriteLine(amountOfRoomsUncheckedAroundMarker + " this shit");

                    //thischunk of code[specifically the underneath whole if statemnet] is supposed to clear shit which might generate so enighbouring rooms have their doors opened or closed depending if the other rooms near them have thyeir doors closed or not
                    if (amountOfRoomsUncheckedAroundMarker == 0)
                    {
                        for (short cycleCleaningRow = 0; cycleCleaningRow < totalYsize; cycleCleaningRow++)
                        {
                            for (short cycleCleaningCell = 0; cycleCleaningCell < totalXSize; cycleCleaningCell++)
                            {
                                if (cycleCleaningRow > 0 && labyrinth[cycleCleaningRow][cycleCleaningCell].north == true && labyrinth[cycleCleaningRow - 1][cycleCleaningCell].south == false)
                                {
                                    labyrinth[cycleCleaningRow][cycleCleaningCell].north = false;
                                }
                                if (cycleCleaningRow < totalYsize - 1 && labyrinth[cycleCleaningRow][cycleCleaningCell].south == true && labyrinth[cycleCleaningRow + 1][cycleCleaningCell].north == false)
                                {
                                    labyrinth[cycleCleaningRow][cycleCleaningCell].south = false;
                                }
                                if (cycleCleaningCell > 0 && labyrinth[cycleCleaningRow][cycleCleaningCell].west == true && labyrinth[cycleCleaningRow][cycleCleaningCell - 1].east == false)
                                {
                                    labyrinth[cycleCleaningRow][cycleCleaningCell].west = false;
                                }
                                if (cycleCleaningCell < totalXSize - 1 && labyrinth[cycleCleaningRow][cycleCleaningCell].east == true && labyrinth[cycleCleaningRow][cycleCleaningCell + 1].west == false)
                                {
                                    labyrinth[cycleCleaningRow][cycleCleaningCell].east = false;
                                }
                            }
                        }
                    }



                    generatingMapForLabyrinth(labyrinth, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller);

                    if (movementType == "P")
                    {
                        gameEndedCheck = gameControlsForMazeGame(labyrinth, false, 0, 0);
                    }
                    else
                    {
                        short coordinateXDes = -2;
                        short coordinateYDes = -2;
                        while ((coordinateXDes < -1 || coordinateXDes > totalXSize - 1 || coordinateYDes < -1 || coordinateYDes > totalYsize - 1) || (coordinateXDes >= 0 && coordinateYDes >= 0 && coordinateXDes <= totalXSize - 1 && coordinateYDes <= totalYsize - 1 && labyrinth[coordinateYDes][coordinateXDes].isThisRoomAllLocked() == true))
                        {
                            try
                            {
                                Console.Write("What X position do you want to land on?");
                                coordinateXDes = Convert.ToInt16(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Please enetr a valid value");
                            }
                            try
                            {
                                Console.Write("What y position do you want to land on?");
                                coordinateYDes = Convert.ToInt16(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Please enetr a valid value");
                            }
                        }


                        if (coordinateXDes > -1 && coordinateYDes > -1)
                        {
                            findBFSFindShortestPath(labyrinth, coordOfOurPlayerPos[1], coordOfOurPlayerPos[0], coordinateXDes, coordinateYDes, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller);

                        }
                        else
                        {
                            gameEndedCheck = 10;
                        }
                    }


                    for (short cycleRow = 0; cycleRow < totalYsize; cycleRow++)
                    {
                        for (short cycleCell = 0; cycleCell < totalXSize; cycleCell++)
                        {

                            try
                            {
                                if (labyrinth[cycleRow][cycleCell].playerPresenceCheck == true)
                                {

                                    short distanceXFromBorder = (short)(cycleCell + 1);
                                    short distanceYFromBorder = (short)(cycleRow + 1);

                                    if (distanceYFromBorder + generatingAreaYSide > totalYsize)
                                    {
                                        for (byte cycleY = 0; cycleY < generatingAreaYSide; cycleY++)
                                        {
                                            labyrinth.Add(new List<Room>());
                                            for (byte cycleX = 0; cycleX < totalXSize; cycleX++)
                                            {
                                                labyrinth[cycleY + totalYsize].Add(new Room(false, false, false, false, false, false));
                                            }

                                        }
                                        totalYsize += generatingAreaYSide;
                                    }
                                    if (distanceYFromBorder <= generatingAreaYSide)
                                    {
                                        for (byte cycleY = 0; cycleY < generatingAreaYSide; cycleY++)
                                        {
                                            labyrinth.Insert(0, new List<Room>());
                                            for (byte cycleX = 0; cycleX < totalXSize; cycleX++)
                                            {
                                                labyrinth[0].Add(new Room(false, false, false, false, false, false));
                                            }
                                        }
                                        totalYsize += generatingAreaYSide;
                                    }
                                    if (distanceXFromBorder + generatingAreaXSide > totalXSize)
                                    {
                                        for (byte cycleY = 0; cycleY < totalYsize; cycleY++)
                                        {
                                            for (byte cycleX = 0; cycleX < generatingAreaXSide; cycleX++)
                                            {
                                                labyrinth[cycleY].Add(new Room(false, false, false, false, false, false));
                                            }
                                        }
                                        totalXSize += generatingAreaXSide;
                                    }
                                    if (distanceXFromBorder <= generatingAreaXSide)
                                    {
                                        for (byte cycleY = 0; cycleY < totalYsize; cycleY++)
                                        {
                                            for (byte cycleX = 0; cycleX < generatingAreaXSide; cycleX++)
                                            {
                                                labyrinth[cycleY].Insert(0, new Room(false, false, false, false, false, false));
                                            }
                                        }
                                        totalXSize += generatingAreaXSide;
                                    }

                                    Console.WriteLine(cycleCell + "     " + cycleRow);





                                }
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }

                        }

                    }






                }
            }
            static short[] coordinatesOfCurPosMarker(List<List<Room>> ourMaze)
            {
                short[] positionOfOurMarker = new short[2];
                for (short cycleY = 0; cycleY < ourMaze.Count; cycleY++)
                {
                    for (short cycleX = 0; cycleX < ourMaze[0].Count; cycleX++)
                    {
                        if (ourMaze[cycleY][cycleX].playerPresenceCheck == true)
                        {
                            positionOfOurMarker[0] = cycleY;
                            positionOfOurMarker[1] = cycleX;
                            return positionOfOurMarker;
                        }
                    }
                }
                return positionOfOurMarker;
            }




























            static byte gameControlsForMazeGame(List<List<Room>> labyrinth, bool isThereEnd, short supposedXStart, short supposedYStart)
            {
                short lengthInLabyrinthX = (short)labyrinth[0].Count;
                short lengthInLabyrinthY = (short)labyrinth.Count;
                string playerCurrentMoveDecision = "X";
                while (playerCurrentMoveDecision != "N" && playerCurrentMoveDecision != "E" && playerCurrentMoveDecision != "S" && playerCurrentMoveDecision != "W" && playerCurrentMoveDecision != "END")
                {
                    Console.Write("\nenter N to move north, enter E to go east, enter S to move south, enter W to move west");
                    playerCurrentMoveDecision = Console.ReadLine();

                }

                if (playerCurrentMoveDecision == "END")
                {
                    //return 5 means you have exited the labyrinth by the beginning or ended the game on purpose by enetering "END", thus u lost(depending on context)
                    return 5;
                }


                short[] coordsOfOurPlayerMarker = coordinatesOfCurPosMarker(labyrinth);

                //position X of player
                short xCoord = coordsOfOurPlayerMarker[1];
                //position y of player
                short yCoord = coordsOfOurPlayerMarker[0];


                try
                {
                    if (playerCurrentMoveDecision == "E" && xCoord <= lengthInLabyrinthX - 1)
                    {
                        if (labyrinth[yCoord][xCoord].east == true)
                        {
                            labyrinth[yCoord][xCoord].playerPresenceCheck = false;
                            labyrinth[yCoord][xCoord + 1].playerPresenceCheck = true;
                            //this is to prevent from going alll the way east
                            playerCurrentMoveDecision = "X";
                        }
                    }
                    else if (playerCurrentMoveDecision == "S" && yCoord <= lengthInLabyrinthY - 1)
                    {

                        if (labyrinth[yCoord][xCoord].south == true)
                        {
                            labyrinth[yCoord][xCoord].playerPresenceCheck = false;
                            labyrinth[yCoord + 1][xCoord].playerPresenceCheck = true;
                            //this is to prevent it from goin g all the way south so i resseted the decison marker to X
                            playerCurrentMoveDecision = "X";
                        }
                    }
                    else if (playerCurrentMoveDecision == "W" && xCoord >= 0)
                    {
                        if (labyrinth[yCoord][xCoord].west == true)
                        {

                            labyrinth[yCoord][xCoord].playerPresenceCheck = false;
                            labyrinth[yCoord][xCoord - 1].playerPresenceCheck = true;
                            playerCurrentMoveDecision = "X";
                        }
                    }
                    else if (playerCurrentMoveDecision == "N" && yCoord >= 0)
                    {
                        if (labyrinth[yCoord][xCoord].north == true)
                        {
                            labyrinth[yCoord][xCoord].playerPresenceCheck = false;
                            labyrinth[yCoord - 1][xCoord].playerPresenceCheck = true;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (xCoord == supposedXStart && yCoord == supposedYStart)
                    {
                        //return 5 means you have exited the labyrinth by the beginning or ended the game on purpose by enetering "END", thus u lost(depending on context)
                        return 5;
                    }
                    Console.WriteLine("You finished the maze game");
                    //1 means you have exited through the actual end, thus you won(Dependes on context tho)
                    return 1;
                }






                //0 means nothing special happened
                return 0;



            }


            //This function is a sub part of the main above program to make my code shorter so i dont have to write teh same lines over and over again
            static void generatingMapForLabyrinth(List<List<Room>> ourMap, string fillerOfEmptyDungeonSpace, string fillerOfWallDungeonArea, string playerCurrentPositionFiller)
            {
                short lengthInLabyrinthX = (short)(ourMap[0].Count);
                short lengthInLabyrinthY = (short)(ourMap.Count);
                string dungeonLabyrinthMap = "";
                for (byte creatingMapRowNewForCurrentPos = 0; creatingMapRowNewForCurrentPos < lengthInLabyrinthY; creatingMapRowNewForCurrentPos++)
                {
                    for (byte identifyingMapSectionOfEachRoom = 0; identifyingMapSectionOfEachRoom < 3; identifyingMapSectionOfEachRoom++)
                    {
                        for (byte creatingMapCellNewForCurrentPos = 0; creatingMapCellNewForCurrentPos < lengthInLabyrinthX; creatingMapCellNewForCurrentPos++)
                        {
                            if (identifyingMapSectionOfEachRoom == 0) { dungeonLabyrinthMap += ourMap[creatingMapRowNewForCurrentPos][creatingMapCellNewForCurrentPos].createRoomMapTopSection(fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea); }
                            else if (identifyingMapSectionOfEachRoom == 1) { dungeonLabyrinthMap += ourMap[creatingMapRowNewForCurrentPos][creatingMapCellNewForCurrentPos].createRoomMapMidSection(fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller); }
                            else { dungeonLabyrinthMap += ourMap[creatingMapRowNewForCurrentPos][creatingMapCellNewForCurrentPos].createRoomMapBotSection(fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea); }
                        }
                        //aads a new line once a row is finsihed
                        dungeonLabyrinthMap += "\n";
                    }

                }
                Console.WriteLine(dungeonLabyrinthMap);
            }
            //im gonna chnage thsoi so there is a chance of the DFS stopping but thas only if limitedGneration is true
            static void depthFirstSearchConstructionLabyrinthBased(List<List<Room>> ourMap, int positionXOfStartingPoint, int positionYOfStartingPoint, string whatDirectioniCameFrom, bool limitedGeneration, byte minimumXAreaGenerated, byte minimumYAreaGenerated, short currentMarkerXPos, short currentMarkerYPos, ifDFSShouldEnd statusDFS)
            {
                //Console.WriteLine(currentMarkerXPos + "    " + currentMarkerYPos+ " are the values of the marker");
                //Uncomment the next 3 line if you wanna see map getting generated at each stag

                /*ourMap[positionYOfStartingPoint][positionXOfStartingPoint].playerPresenceCheck = true;
                generatingMapForLabyrinth(ourMap, "  ", "{}", "XX");
                ourMap[positionYOfStartingPoint][positionXOfStartingPoint].playerPresenceCheck = false;*/

                bool temporaryAllowanceForGeneration = !(limitedGeneration);
                short YSizeOfMaze = (short)(ourMap.Count);
                short XSizeOfMaze = (short)(ourMap[0].Count);
                short amountOfRoomsUncheckedAroundMarker = 0;
                if (limitedGeneration == true)
                {
                    for (short cycleRow = 0; cycleRow < YSizeOfMaze; cycleRow++)
                    {
                        for (short cycleCell = 0; cycleCell < XSizeOfMaze; cycleCell++)
                        {

                            if (absoluteVal(cycleRow - currentMarkerYPos) <= minimumYAreaGenerated && absoluteVal(cycleCell - currentMarkerXPos) <= minimumXAreaGenerated && ourMap[cycleRow][cycleCell].checkForDoorOpeningStability == false)
                            {
                                temporaryAllowanceForGeneration = true;
                                //Console.WriteLine(cycleCell+"    and    "+cycleRow);
                                amountOfRoomsUncheckedAroundMarker++;
                            }

                        }
                    }
                    if (statusDFS.generationRegularity > 0)
                    {
                        Random chanceOfStopping = new Random();

                        Console.WriteLine(statusDFS.generationRegularity + "   jeeez");

                        short stopChosneNotOrYes = (short)chanceOfStopping.Next(0, (short)Math.Pow(statusDFS.generationRegularity, 2));


                        if (stopChosneNotOrYes == 0)
                        {
                            statusDFS.generationRegularity = 1;
                            statusDFS.update();

                        }


                    }





                }

                if (statusDFS.generationRegularity == 1)
                {
                    Console.WriteLine("please stop at " + positionXOfStartingPoint + "   " + positionYOfStartingPoint);

                }

                if (temporaryAllowanceForGeneration == true)
                {
                    Console.WriteLine("conducting DFS at " + positionXOfStartingPoint + "    " + positionYOfStartingPoint);
                    ourMap[positionYOfStartingPoint][positionXOfStartingPoint].checkForDoorOpeningStability = true;

                    if (whatDirectioniCameFrom == "N")//2 associated with: The selector came from their north door to my current south door
                    {
                        ourMap[positionYOfStartingPoint][positionXOfStartingPoint].north = true;
                    }
                    else if (whatDirectioniCameFrom == "S")//0 asscoiated with: the slector came from their south door to my current north door
                    {
                        ourMap[positionYOfStartingPoint][positionXOfStartingPoint].south = true;
                    }
                    else if (whatDirectioniCameFrom == "W")//3 associated with: the slecetor caem from their east door to my current east door
                    {
                        ourMap[positionYOfStartingPoint][positionXOfStartingPoint].west = true;
                    }
                    else if (whatDirectioniCameFrom == "E")//1 associated with: the sleector came from their west door to my currente east
                    {
                        ourMap[positionYOfStartingPoint][positionXOfStartingPoint].east = true;
                    }

                    if (statusDFS.continueAllowance == false)
                    {
                        return;
                    }

                    //I made a random instance of the Random class because its a way to detremine which path should be checked first 
                    Random whichPathDoWeFirstSelect = new Random();
                    bool Path1Checked = false;
                    bool Path2Checked = false;
                    bool Path3Checked = false;
                    bool Path4Checked = false;
                    while (Path1Checked == false || Path2Checked == false || Path3Checked == false || Path4Checked == false)
                    {

                        //List<List<int>> myListOfAdjacentCells = neighboursFindingSystemLabyrinthBased(ourMap, positionXOfStartingPoint, positionYOfStartingPoint, XSizeOfMaze, YSizeOfMaze);

                        int whatPathDidWeChoose = whichPathDoWeFirstSelect.Next(0, 4);
                        //if Path 0 is the one we are going to view 1st
                        if (whatPathDidWeChoose == 0)
                        {
                            //is there any room beneath us
                            if (positionYOfStartingPoint < YSizeOfMaze - 1)
                            {
                                if (ourMap[positionYOfStartingPoint + 1][positionXOfStartingPoint].checkForDoorOpeningStability == false)
                                {
                                    //applying DepthForSearchLabyrinthBased[DFS-MazeBased] to value N means it came from south[of currents sector posiion] my next vist at north[Room Sector North of Next Room]
                                    /*
                                    *   ⌄⌄⌄
                                    * {[y][x]}
                                    *   ⌄⌄⌄
                                    * {[y+1][x]}
                                    */
                                    ourMap[positionYOfStartingPoint][positionXOfStartingPoint].south = true;
                                    //Uncomment the next two lines if you wanna see details while genrating the Map
                                    //Console.WriteLine(whatDirectioniCameFrom+" Is where the selector slected fom");
                                    //Console.WriteLine(whatPathDidWeChoose+" Is what Path has been viewed first");
                                    depthFirstSearchConstructionLabyrinthBased(ourMap, positionXOfStartingPoint, positionYOfStartingPoint + 1, "N", limitedGeneration, minimumXAreaGenerated, minimumYAreaGenerated, currentMarkerXPos, currentMarkerYPos, statusDFS);
                                }
                            }
                            Path1Checked = true;
                        }
                        //if Path 1 is the one we are going to view 1st
                        else if (whatPathDidWeChoose == 1)
                        {
                            //is there any room above us
                            if (positionYOfStartingPoint > 0)
                            {
                                if (ourMap[positionYOfStartingPoint - 1][positionXOfStartingPoint].checkForDoorOpeningStability == false)
                                {
                                    //applying DepthForSearchLabyrinthBased[DFS-MazeBased] to value S means it came from north[of current sector position] to my next vist at south[Room scetor South of next room]
                                    /*
                                     * {[y-1][x]}
                                     *     ^^^
                                     * {[y][x]}
                                     *     ^^^
                                     */
                                    ourMap[positionYOfStartingPoint][positionXOfStartingPoint].north = true;
                                    //Uncomment the next two lines if you wanna see details while genrating the Map
                                    //Console.WriteLine(whatDirectioniCameFrom + " Is where the selector slected fom");
                                    //Console.WriteLine(whatPathDidWeChoose + " Is what Path has been viewed first");
                                    depthFirstSearchConstructionLabyrinthBased(ourMap, positionXOfStartingPoint, positionYOfStartingPoint - 1, "S", limitedGeneration, minimumXAreaGenerated, minimumYAreaGenerated, currentMarkerXPos, currentMarkerYPos, statusDFS);
                                }


                            }
                            Path2Checked = true;
                        }

                        //if Path 2 is the one we are going to view 1st
                        else if (whatPathDidWeChoose == 2)
                        {
                            //is there any rooms on our east[right]
                            if (positionXOfStartingPoint < XSizeOfMaze - 1)
                            {
                                //applying DepthForSearchLabyrinthBased[DFS-MazeBased] to value W means it came from east[currentd door of our position] to my next vist at west[Room sector West of nmext room]
                                /*  
                                 *  {Arrow coming from west of current room to east sector of next room}
                                 *  => {[y][x]} => {[y][x+1]}
                                 *  
                                  */
                                if (ourMap[positionYOfStartingPoint][positionXOfStartingPoint + 1].checkForDoorOpeningStability == false)
                                {
                                    ourMap[positionYOfStartingPoint][positionXOfStartingPoint].east = true;
                                    //Uncomment the next two lines if you wanna see details while genrating the Map
                                    //Console.WriteLine(whatDirectioniCameFrom + " Is where the selector slected fom");
                                    //Console.WriteLine(whatPathDidWeChoose + " Is what Path has been viewed first");
                                    depthFirstSearchConstructionLabyrinthBased(ourMap, positionXOfStartingPoint + 1, positionYOfStartingPoint, "W", limitedGeneration, minimumXAreaGenerated, minimumYAreaGenerated, currentMarkerXPos, currentMarkerYPos, statusDFS);
                                }
                            }
                            Path3Checked = true;
                        }
                        //if Path 3 is the one we are going to view 1st
                        else if (whatPathDidWeChoose == 3)
                        {
                            //is there any room on our west[left]
                            if (positionXOfStartingPoint > 0)
                            {
                                //applying DepthForSearchLabyrinthBased[DFS-MazeBased] to value E means it came from west[currentd door of our position] to my next vist at east[Room sector East of next room]
                                /*
                                 * {Arrow coming from west sector of current Room to east sector of next room}
                                 * {[y][x-1]} <= {[y][x]} <=
                                 * 
                                 */
                                if (ourMap[positionYOfStartingPoint][positionXOfStartingPoint - 1].checkForDoorOpeningStability == false)
                                {
                                    ourMap[positionYOfStartingPoint][positionXOfStartingPoint].west = true;
                                    //Uncomment the next two lines if you wanna see details while genrating the Map
                                    //Console.WriteLine(whatDirectioniCameFrom + " Is where the selector slected fom");
                                    //Console.WriteLine(whatPathDidWeChoose + " Is what Path has been viewed first");
                                    //Runs DFS on the neighbour which is at our current East
                                    depthFirstSearchConstructionLabyrinthBased(ourMap, positionXOfStartingPoint - 1, positionYOfStartingPoint, "E", limitedGeneration, minimumXAreaGenerated, minimumYAreaGenerated, currentMarkerXPos, currentMarkerYPos, statusDFS);
                                }
                            }
                            Path4Checked = true;
                        }
                    }
                }








            }

            static short absoluteVal(int ourVal)
            {
                return (short)Math.Pow(Math.Pow(ourVal, 2), 0.5);
            }

            static bool labyrinthFiniteVersionGeneratorGame(short lengthInLabyrinthX, short lengthInLabyrinthY, string fillerOfEmptyDungeonSpace, string fillerOfWallDungeonArea, string playerCurrentPositionFiller, short XCoordStart, short YCoordStart, short XCoordEnd, short YCoordEnd)
            {
                string typeOfLabyrinthGamePlayer = "null";
                while (typeOfLabyrinthGamePlayer != "P" && typeOfLabyrinthGamePlayer != "AI")
                {
                    Console.Write("Do you the labyrinth game to be played by Articficial Intel or Real Player\nP for Player, AI for Artificial Intel");
                    typeOfLabyrinthGamePlayer = Console.ReadLine();

                }
                List<List<Room>> labyrinth = new List<List<Room>>();
                for (short labyrinthConstructionInEachRow = 0; labyrinthConstructionInEachRow < lengthInLabyrinthY; labyrinthConstructionInEachRow++)
                {
                    labyrinth.Add(new List<Room> { });

                    for (short labyrinthConstructionFillingEachCell = 0; labyrinthConstructionFillingEachCell < lengthInLabyrinthX; labyrinthConstructionFillingEachCell++)
                    {

                        labyrinth[labyrinthConstructionInEachRow].Add(new Room(false, false, false, false, false, false));
                    }
                }


                Random randomCoords = new Random();
                short ourRandomCoordinates = (short)randomCoords.Next(0, lengthInLabyrinthX);
                if (XCoordStart < 0 || XCoordStart > lengthInLabyrinthX - 1)
                {
                    XCoordStart = ourRandomCoordinates;
                }
                ourRandomCoordinates = (short)randomCoords.Next(0, lengthInLabyrinthY);
                if (YCoordStart < 0 || YCoordStart > lengthInLabyrinthY - 1)
                {
                    YCoordStart = ourRandomCoordinates;
                }
                //this do while loops is just to make sure the start and beginning coords labyrinth[y][x]/graph(x,y) are not the same
                do
                {
                    ourRandomCoordinates = (short)randomCoords.Next(0, lengthInLabyrinthX);
                    if (XCoordEnd < 0 || XCoordEnd > lengthInLabyrinthX - 1)
                    {
                        XCoordEnd = ourRandomCoordinates;
                    }
                    ourRandomCoordinates = (short)randomCoords.Next(0, lengthInLabyrinthY);
                    if (YCoordEnd < 0 || YCoordEnd > lengthInLabyrinthY - 1)
                    {
                        YCoordEnd = ourRandomCoordinates;
                    }
                } while (XCoordStart == XCoordEnd && YCoordStart == YCoordEnd);

                // the next following lines are to chekc if the startin and endin coordsinates are at the border and if they are then a path will brokwn outta a direction depnding on the situation
                List<string> possibleOriginDirections = new List<string>();
                List<string> possibleEndPaths = new List<string>();

                Console.WriteLine("We will start at {" + XCoordStart + "   " + YCoordStart + "}\nAnd we will end at {" + XCoordEnd + "   " + YCoordEnd + "}");



                if (XCoordStart == 0) { possibleOriginDirections.Add("W"); }
                if (XCoordStart == lengthInLabyrinthX - 1) { possibleOriginDirections.Add("E"); }
                if (YCoordStart == 0) { possibleOriginDirections.Add("N"); }
                if (YCoordStart == lengthInLabyrinthY - 1) { possibleOriginDirections.Add("S"); }
                if (XCoordEnd == 0) { possibleEndPaths.Add("W"); }
                if (XCoordEnd == lengthInLabyrinthX - 1) { possibleEndPaths.Add("E"); }
                if (YCoordEnd == 0) { possibleEndPaths.Add("N"); }
                if (YCoordEnd == lengthInLabyrinthY - 1) { possibleEndPaths.Add("S"); }

                Random whichDoorSelector = new Random();

                string chosenStartDoor = "X";
                if (possibleOriginDirections.Count > 0)
                {
                    chosenStartDoor = possibleOriginDirections[whichDoorSelector.Next(0, possibleOriginDirections.Count)];
                }

                string chosenEndDoor = "X";
                if (possibleEndPaths.Count > 0)
                {
                    chosenEndDoor = possibleEndPaths[whichDoorSelector.Next(0, possibleEndPaths.Count)];
                    labyrinth[YCoordEnd][XCoordEnd].quickSingleDoorOpen(chosenEndDoor);
                }
                //===================



                //applies DFS on the labyrinth from position Labyrinth[yStart][xStart]     
                ifDFSShouldEnd neutralStatusOfDFS = new ifDFSShouldEnd(0);
                depthFirstSearchConstructionLabyrinthBased(labyrinth, XCoordStart, YCoordStart, chosenStartDoor, false, 0, 0, 0, 0, neutralStatusOfDFS);

                labyrinth[YCoordStart][XCoordStart].playerPresenceCheck = true;




                //opens the south gate/sector of the labyrinth[yEnd][xEnd]
                if (typeOfLabyrinthGamePlayer == "P")
                {

                    //movement interferance starts here
                    Console.WriteLine("\n\nGamse Started:\n\n======================\n\n======================\n\n");



                    byte gameEndedCheck = 0;
                    while (gameEndedCheck != 10 && gameEndedCheck != 5)
                    {

                        generatingMapForLabyrinth(labyrinth, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller);



                        gameEndedCheck = gameControlsForMazeGame(labyrinth, true, XCoordStart, YCoordStart);
                        if (gameEndedCheck == 1) { return true; }

                        short[] playerCoords = coordinatesOfCurPosMarker(labyrinth);
                        short xPos = playerCoords[1];
                        short yPos = playerCoords[0];

                        //the following line of code is if the end coordinate of teh whole labyrinth is not on the borders of the labyrinth, so people who reach the internal exit still win cause its still an exit after all

                        Console.WriteLine("your currently at {" + xPos + "  as X coordinate, " + yPos + " as y coordinate}");
                        if (xPos == XCoordEnd && yPos == YCoordEnd && XCoordEnd < lengthInLabyrinthX - 1 && YCoordEnd < lengthInLabyrinthY - 1)
                        {
                            Console.WriteLine("\n\n\nWell done you have reached the end, u lucky twat\n atleast your not that retarded you cant even solve a fucking maze pfft-");
                            gameEndedCheck = 10;
                        }
                    }
                    if (gameEndedCheck == 10)
                    {
                        return true;
                    }
                    //the follwoing line wont even get executed if the line above gets executed first
                    Console.WriteLine("\n\n\nSorry but you are mistake in this universe,\nConsider your life decisions you made so far, YOU fuCkiNg loSEr!!!");
                    return false;

                }




                if (typeOfLabyrinthGamePlayer == "AI")
                {


                    //generatingMapForLabyrinth(labyrinth, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller, lengthInLabyrinthX, lengthInLabyrinthY);
                    findBFSFindShortestPath(labyrinth, XCoordStart, YCoordStart, XCoordEnd, YCoordEnd, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller);
                }

                //place holder
                Console.WriteLine("\n\n\nYou sooo dumb that chu had to bloody use the ai to solve thsi for you\nBiggest Bruh...Sister that wasnt very cash money of you shmh~");
                return false;
            }
            //watch https://www.youtube.com/watch?v=xlVX7dXLS64&t=1s&pbjreload=101 To see why I made this function
            static List<List<int>> breadthFirstSearchingPathLabyrinthBased(List<List<Room>> ourMaze, short coordinateXOfStart, short coordinateYOfStart, short coordinateXForEnd, short coordinateYForEnd)
            {
                short mazeXSize = (short)ourMaze[0].Count;
                short mazeYsize = (short)ourMaze.Count;
                List<List<int>> reccomendedCoords = new List<List<int>>();
                for (short cycleRow = 0; cycleRow < mazeYsize; cycleRow++)
                {
                    for (short cycleCell = 0; cycleCell < mazeXSize; cycleCell++)
                    {
                        ourMaze[cycleRow][cycleCell].checkForDoorOpeningStability = false;
                    }
                }
                //put in the structure of ===>    new List<short>{Y[intentional to have y val], X[inetntional too]}
                reccomendedCoords.Add(new List<int> { coordinateYOfStart, coordinateXOfStart });

                //creates a array of empty slots[each slot is supposed to hold another array in whihc x and y coordinates are stored of each room] 
                List<List<int>> rebuildOrginPath = new List<List<int>>();



                while (reccomendedCoords.Count > 0)
                {
                    //Uncomment to see details of the Recommended List
                    string reccomendedWrittenCoords = "";
                    foreach (var cycleOfReccomendations in reccomendedCoords)
                    {
                        reccomendedWrittenCoords += "{" + cycleOfReccomendations[1] + " X ," + cycleOfReccomendations[0] + " Y }  ";
                    }
                    Console.WriteLine(reccomendedWrittenCoords + "   is the list of our unamrked positions we should next visit\n\n");

                    List<int> nextVisit = reccomendedCoords[0];
                    List<List<int>> myListOfAdjacentCells = neighboursFindingSystemLabyrinthBased(ourMaze, nextVisit[1], nextVisit[0]);
                    reccomendedCoords.RemoveAt(0);

                    if (ourMaze[nextVisit[0]][nextVisit[1]].checkForDoorOpeningStability == false)
                    {
                        ourMaze[nextVisit[0]][nextVisit[1]].checkForDoorOpeningStability = true;
                        //Uncomment if you want to see how the AI uses BFS to go arund the labyrinth
                        //ourMaze[nextVisit[0]][nextVisit[1]].playerPresenceCheck = true;
                        //generatingMapForLabyrinth(ourMaze, "--", "{}", "XX", mazeXSize, mazeYsize);
                        //ourMaze[nextVisit[0]][nextVisit[1]].playerPresenceCheck = false;








                        foreach (var cycleOfAdjacency in myListOfAdjacentCells)
                        {

                            if (ourMaze[cycleOfAdjacency[0]][cycleOfAdjacency[1]].checkForDoorOpeningStability == false)
                            {
                                Console.WriteLine(cycleOfAdjacency[1] + "    " + cycleOfAdjacency[0]);

                                reccomendedCoords.Add(new List<int> { cycleOfAdjacency[0], cycleOfAdjacency[1] });

                                rebuildOrginPath.Add(new List<int> { cycleOfAdjacency[0], cycleOfAdjacency[1] });

                            }

                            if (cycleOfAdjacency[1] == coordinateXForEnd && cycleOfAdjacency[0] == coordinateYForEnd)
                            {
                                return rebuildOrginPath;
                            }
                        }




                    }

                }

                return rebuildOrginPath;
            }
            static List<List<int>> neighboursFindingSystemLabyrinthBased(List<List<Room>> ourMaze, int currentXPosition, int currentYPosition)
            {
                short mazeXSize = (short)ourMaze[0].Count;
                short mazeYSize = (short)ourMaze.Count;
                List<List<int>> myListOfNeighbours = new List<List<int>>();
                if (currentYPosition > 0 && ourMaze[currentYPosition][currentXPosition].north == true)
                {
                    myListOfNeighbours.Add(new List<int> { currentYPosition - 1, currentXPosition });


                }
                if (currentYPosition < mazeYSize - 1 && ourMaze[currentYPosition][currentXPosition].south == true)
                {
                    myListOfNeighbours.Add(new List<int> { currentYPosition + 1, currentXPosition });
                }

                if (currentXPosition > 0 && ourMaze[currentYPosition][currentXPosition].west == true)
                {
                    myListOfNeighbours.Add(new List<int> { currentYPosition, currentXPosition - 1 });
                }

                if (currentXPosition < mazeXSize - 1 && ourMaze[currentYPosition][currentXPosition].east == true)
                {
                    myListOfNeighbours.Add(new List<int> { currentYPosition, currentXPosition + 1 });
                }

                //Uncomment below to see the neighbour list 
                /*string myListOfNeighboursPaper = "";
                foreach (var cycle in myListOfNeighbours)
                {
                    myListOfNeighboursPaper += "{" + cycle[1] + " x ," + cycle[0] + " y }  ";
                }
                
                Console.WriteLine("current cell is {"+currentXPosition+"  as x "+currentYPosition+" as y}\n"+myListOfNeighboursPaper+"  Are the neighbours of the current cell\n\n");*/
                return myListOfNeighbours;

            }
            static void findBFSFindShortestPath(List<List<Room>> ourLabyrinth, short coordinateXOfStart, short coordinateYOfStart, short coordinateXOfEnd, short coordinateYForEnd, string fillerOfEmptyDungeonSpace, string fillerOfWallDungeonArea, string playerCurrentPositionFiller)
            {
                List<List<int>> rebuildOrginPath = breadthFirstSearchingPathLabyrinthBased(ourLabyrinth, coordinateXOfStart, coordinateYOfStart, coordinateXOfEnd, coordinateYForEnd);

                List<List<short>> pathBackTrackedAndFlipped = reBuildBFSPathLabyrinthBased(ourLabyrinth, coordinateXOfStart, coordinateYOfStart, Convert.ToInt16(coordinateXOfEnd), Convert.ToInt16(coordinateYForEnd), rebuildOrginPath);

                for (short cycleOfPathWalk = 0; cycleOfPathWalk < pathBackTrackedAndFlipped.Count; cycleOfPathWalk++)
                {
                    ourLabyrinth[pathBackTrackedAndFlipped[cycleOfPathWalk][0]][pathBackTrackedAndFlipped[cycleOfPathWalk][1]].playerPresenceCheck = true;
                    generatingMapForLabyrinth(ourLabyrinth, fillerOfEmptyDungeonSpace, fillerOfWallDungeonArea, playerCurrentPositionFiller);
                    if (cycleOfPathWalk < pathBackTrackedAndFlipped.Count - 1)
                    {
                        ourLabyrinth[pathBackTrackedAndFlipped[cycleOfPathWalk][0]][pathBackTrackedAndFlipped[cycleOfPathWalk][1]].playerPresenceCheck = false;
                    }

                }

            }
            static List<List<short>> reBuildBFSPathLabyrinthBased(List<List<Room>> ourLabyrinth, short coordinateXOfStart, short coordinateYOfStart, short coordinateXOfEnd, short coordinateYForEnd, List<List<int>> rebuildOrginPath)
            {
                List<List<short>> shortestPath = new List<List<short>>();
                List<int> destination = new List<int>();


                destination.Add(coordinateYForEnd);
                destination.Add(coordinateXOfEnd);
                //Console.WriteLine(destination[0] + "   " + destination[1]);


                rebuildOrginPath.Insert(0, new List<int> { coordinateYOfStart, coordinateXOfStart });

                /*short cycleOfPathBuild = 0;
                while (destination[0] != -1)
                {
                    shortestPath.Add(destination);
                    destination.Clear();
                    
                    destination.Add(rebuildOrginPath[cycleOfPathBuild][0]);
                    destination.Add(rebuildOrginPath[cycleOfPathBuild][1]);
                    cycleOfPathBuild++;
                        
                }*/


                for (short cycle = 0; cycle < rebuildOrginPath.Count; cycle++)
                {
                    //Console.WriteLine("{" + rebuildOrginPath[cycle][1] + " is the x coordinate   " + rebuildOrginPath[cycle][0] + " is teh y coordinate}");
                    short currentAddress = cycle;
                    if (cycle > 0 && rebuildOrginPath[cycle][1] == coordinateXOfEnd && rebuildOrginPath[cycle][0] == coordinateYForEnd)
                    {

                        short neighbourAdress = cycle;

                        foreach (var checkingWhichNeighbourCell in rebuildOrginPath)
                        {

                            /*Sample:{50,52}                  Invalid vectors:
                                              {52,52}     {50,50}     {48,52}     {50,54}
                                              xV>+1        yV<-1        xV<-1       yV>+1
                                              [>1 0]     [0  <-1]     [<-1  0]     [0  >1]  
                            
                            
                            Valid Vectors:
                            {50, 53}  (0, +1)
                            {51, 52}  (+1, 0)
                            {50, 51}  (0, -1)
                            {49, 52}  (-1, 0)
                            
                            
                            
                            We aslo want top check
                            */



                            if (neighbourAdress > 0)
                            {
                                short adressValueXCur = (short)rebuildOrginPath[currentAddress][1];
                                short adressValueYCur = (short)rebuildOrginPath[currentAddress][0];
                                short adressValueXNeighbour = (short)rebuildOrginPath[neighbourAdress - 1][1];
                                short adressValueYNeighbour = (short)rebuildOrginPath[neighbourAdress - 1][0];
                                Console.WriteLine("{" + rebuildOrginPath[currentAddress][1] + " x value and " + rebuildOrginPath[currentAddress][0] + " y value}  {" + rebuildOrginPath[neighbourAdress - 1][1] + " is the x value of the one we are comparing and " + rebuildOrginPath[neighbourAdress - 1][0] + " is its y value}");
                                if (areTheseTwoCellsValidNeighbours(ourLabyrinth, adressValueXCur, adressValueYCur, adressValueXNeighbour, adressValueYNeighbour) == true)
                                {
                                    shortestPath.Add(new List<short> { adressValueYNeighbour, adressValueXNeighbour });
                                    currentAddress = (short)(neighbourAdress - 1);
                                }

                            }
                            neighbourAdress--;


                        }

                    }

                    /*if (cycle % 4 == 0)
                    {
                        Console.WriteLine("\n");
                    }*/
                }
                //I am reversing the list of of coordinates leadinbg to the shortest path beacuse i used back tracking to find the way back to the start and its necessary to ebe revsred if i want the path from Start=>End instead of having a path of End=>Start{Because it was back tracked}
                shortestPath.Reverse();
                //this is because i only back tracked from the End nopt actiually including teh End coordinaet to my accumulation, thus i had to manually add it to get the full path, i also removed the start coordinate cause i assumed the playerMArker would be already at the start Room coordinate

                shortestPath.Add(new List<short> { coordinateYForEnd, coordinateXOfEnd });
                foreach (var cycleOfShowingPath in shortestPath)
                {
                    Console.WriteLine("{" + cycleOfShowingPath[1] + " is the x coordinate   " + cycleOfShowingPath[0] + " is teh y coordinate}");
                }
                Console.WriteLine(rebuildOrginPath.Count);
                return shortestPath;

            }

            static bool areTheseTwoCellsValidNeighbours(List<List<Room>> ourMaze, short adressValueXCur, short adressValueYCur, short adressValueXNeighbour, short adressValueYNeighbour)
            {

                if (adressValueYCur - adressValueYNeighbour == 1 && adressValueXCur - adressValueXNeighbour == 0 && ourMaze[adressValueYCur][adressValueXCur].north == true && ourMaze[adressValueYNeighbour][adressValueXNeighbour].south == true)
                {
                    return true;
                }
                if (adressValueYCur - adressValueYNeighbour == 0 && adressValueXCur - adressValueXNeighbour == 1 && ourMaze[adressValueYCur][adressValueXCur].west == true && ourMaze[adressValueYNeighbour][adressValueXNeighbour].east == true)
                {
                    return true;
                }
                if (adressValueYCur - adressValueYNeighbour == -1 && adressValueXCur - adressValueXNeighbour == 0 && ourMaze[adressValueYCur][adressValueXCur].south == true && ourMaze[adressValueYNeighbour][adressValueXNeighbour].north == true)
                {
                    return true;
                }
                if (adressValueYCur - adressValueYNeighbour == 0 && adressValueXCur - adressValueXNeighbour == -1 && ourMaze[adressValueYCur][adressValueXCur].east == true && ourMaze[adressValueYNeighbour][adressValueXNeighbour].west == true)
                {
                    return true;
                }
                return false;
            }






































































            static string makeTurnBasedGame(byte allyCount, byte enemyCount, short minAllWarriorsHP, short maxAllWarriorHP)
            {
                //creates 2 lists and each contain the cuuurentHealth Variable in the instances of standardWarrior
                standardWarrior[] allyArmy = new standardWarrior[allyCount];
                standardWarrior[] enemyArmy = new standardWarrior[enemyCount];
                for (byte creatingAllyArmy = 0; creatingAllyArmy < allyCount; creatingAllyArmy++)
                {
                    allyArmy[creatingAllyArmy] = new standardWarrior(minAllWarriorsHP, maxAllWarriorHP);
                }
                for (byte creatingEnemyArmy = 0; creatingEnemyArmy < enemyCount; creatingEnemyArmy++)
                {
                    enemyArmy[creatingEnemyArmy] = new standardWarrior(minAllWarriorsHP, maxAllWarriorHP);
                }
                string gameTypeSelect = "null";
                while (gameTypeSelect != "PvP" && gameTypeSelect != "PvAI")
                {
                    Console.Write("Enetr what game mode you wish to play");
                    gameTypeSelect = Console.ReadLine();
                }
                bool playerOneTurn = false;
                bool playerTwoTurn = false;
                Random whichPlayerStartsDecision = new Random();
                if (playerOneTurn == false && playerTwoTurn == false)
                {
                    if (whichPlayerStartsDecision.Next(0, 2) == 0)
                    {
                        playerOneTurn = true;
                    }
                    else
                    {
                        playerTwoTurn = true;
                    }
                }
                //player2 is basically enemy and player1 is ally
                bool gameEndCheck = false;
                while (gameEndCheck == false)
                {


                    byte defenceSuccessActedUpon = 1;
                    bool currentPlayerActionValidCheck = false;
                    byte foundDeadAlly = 0;
                    for (byte checkingForDeadAllies = 0; checkingForDeadAllies < allyArmy.Length; checkingForDeadAllies++)
                    {
                        if (allyArmy[checkingForDeadAllies].currentWarriorHealth <= 0)
                        {
                            foundDeadAlly++;
                        }
                    }
                    byte foundDeadEnemy = 0;
                    for (byte checkingForDeadEnemies = 0; checkingForDeadEnemies < enemyArmy.Length; checkingForDeadEnemies++)
                    {
                        if (enemyArmy[checkingForDeadEnemies].currentWarriorHealth <= 0)
                        {
                            foundDeadEnemy++;
                        }
                    }
                    //if the amount of dead allies meets the amount of ppl in the army the game ends, same for the enmey side
                    if (allyArmy.Length == foundDeadAlly)
                    {
                        gameEndCheck = true;
                        return "player1won";

                    }
                    else if (enemyArmy.Length == foundDeadEnemy)
                    {
                        gameEndCheck = true;
                        return "player2won";
                    }
                    else if (enemyArmy.Length == foundDeadEnemy && allyArmy.Length == foundDeadAlly)
                    {
                        return "draw";
                    }
                    while (currentPlayerActionValidCheck == false && defenceSuccessActedUpon > 0)
                    {
                        if (defenceSuccessActedUpon > 0)
                        {
                            defenceSuccessActedUpon--;
                        }
                        string choosingTurnAction = "null";
                        if ((gameTypeSelect == "PvP") || (gameTypeSelect == "PvAI" && playerOneTurn == true))
                        {
                            while (choosingTurnAction != "A" && choosingTurnAction != "D" && choosingTurnAction != "H")
                            {
                                Console.Write("Eneter A to Attack, D to defend, H to heal");
                                choosingTurnAction = Console.ReadLine();
                            }
                        }
                        else if (gameTypeSelect == "PvAI" && playerTwoTurn == true)
                        {
                            Random probabilityOfDefendOrNotDefend = new Random();
                            byte chosenDefendOrNotDefend = (byte)probabilityOfDefendOrNotDefend.Next(0, 3);
                            if (chosenDefendOrNotDefend == 0) { choosingTurnAction = "D"; }
                            if (choosingTurnAction != "D")
                            {
                                Random whatIfHealAndAtkSameProb = new Random();
                                byte probabilityOfDfnOrAtk = (byte)(((float)enemyArmy.Length / (float)foundDeadEnemy) * 100);
                                if (probabilityOfDfnOrAtk > 50 && probabilityOfDfnOrAtk <= 100) { choosingTurnAction = "H"; }
                                else if (probabilityOfDfnOrAtk < 50 && probabilityOfDfnOrAtk >= 0) { choosingTurnAction = "A"; }
                                else
                                {
                                    byte weChoseHealOrAtk = (byte)whatIfHealAndAtkSameProb.Next(0, 2);
                                    if (weChoseHealOrAtk == 0)
                                    {
                                        choosingTurnAction = "H";
                                    }
                                    else if (weChoseHealOrAtk == 1)
                                    {
                                        choosingTurnAction = "A";
                                    }

                                }
                            }
                        }

                        //if the action type chosen is not defend
                        if (choosingTurnAction != "D")
                        {
                            short whichUnitToActUpon = -1;
                            //prints the target army sample depending on which player's turn it is and if the player chsoe heal or attack
                            if (gameTypeSelect == "PvP" || (gameTypeSelect == "PvAI" && playerOneTurn == true))
                            {
                                while ((((playerOneTurn == true && choosingTurnAction == "A") || (playerTwoTurn == true && choosingTurnAction == "H")) && (whichUnitToActUpon < 0 || whichUnitToActUpon > enemyArmy.Length)) || (((playerTwoTurn == true && choosingTurnAction == "A") || (playerOneTurn == true && choosingTurnAction == "H")) && (whichUnitToActUpon < 0 || whichUnitToActUpon > allyArmy.Length)))
                                {
                                    string creatingLocationMap = "";
                                    //prints the enemy army map if its player1s turn and they are attacking or if its player2s turn and they are healing
                                    if ((playerOneTurn == true && choosingTurnAction == "A") || (playerTwoTurn == true && choosingTurnAction == "H"))
                                    {

                                        for (byte showingEnemyLocation = 0; showingEnemyLocation < enemyArmy.Length; showingEnemyLocation++)
                                        {

                                            creatingLocationMap += enemyArmy[showingEnemyLocation].currentWarriorHealth + "       ";
                                        }


                                    }
                                    //prints the ally army map if its player2s turn and they are attacking or if its player1s turn and they are healing
                                    else if ((playerTwoTurn == true && choosingTurnAction == "A") || (playerOneTurn == true && choosingTurnAction == "H"))
                                    {
                                        for (byte showingAllyLocation = 0; showingAllyLocation < allyArmy.Length; showingAllyLocation++)
                                        {
                                            creatingLocationMap += allyArmy[showingAllyLocation].currentWarriorHealth + "       ";
                                        }
                                    }
                                    Console.WriteLine(creatingLocationMap);
                                    //if the location/coordinate entred is not valid, it catches the mistake and logs out wrror in teh console
                                    try
                                    {
                                        Console.Write(" Which location do you want to carry out your action");
                                        whichUnitToActUpon = Convert.ToInt16(Console.ReadLine());
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Please enetr a valid loaction");
                                    }
                                    Console.WriteLine(whichUnitToActUpon);
                                    if (defenceSuccessActedUpon == 0)
                                    {
                                        currentPlayerActionValidCheck = true;
                                    }


                                }
                            }
                            else if (gameTypeSelect == "PvAI" && playerTwoTurn == true)
                            {
                                //here chooses who teh bot should heal/atk
                                whichUnitToActUpon = 0;
                                if (choosingTurnAction == "A")
                                {
                                    for (byte lookingForWeakestAllyToAttack = 0; lookingForWeakestAllyToAttack < allyArmy.Length; lookingForWeakestAllyToAttack++)
                                    {
                                        if (lookingForWeakestAllyToAttack > 0 && allyArmy[lookingForWeakestAllyToAttack - 1].currentWarriorHealth < allyArmy[lookingForWeakestAllyToAttack].currentWarriorHealth && allyArmy[lookingForWeakestAllyToAttack].currentWarriorHealth > 0)
                                        {
                                            whichUnitToActUpon = lookingForWeakestAllyToAttack;


                                        }
                                    }
                                    string creatingTheMapThatShowDmgThatWasDltByAI = "";
                                    for (byte possibleResOfAIAtk = 0; possibleResOfAIAtk < allyArmy.Length; possibleResOfAIAtk++)
                                    {
                                        creatingTheMapThatShowDmgThatWasDltByAI += allyArmy[possibleResOfAIAtk].currentWarriorHealth + "       ";
                                    }
                                    Console.WriteLine(creatingTheMapThatShowDmgThatWasDltByAI);
                                }
                                else if (choosingTurnAction == "H")
                                {
                                    for (byte lookingForWeakestEnemyToHeal = 0; lookingForWeakestEnemyToHeal < enemyArmy.Length; lookingForWeakestEnemyToHeal++)
                                    {
                                        if (lookingForWeakestEnemyToHeal > 0 && enemyArmy[lookingForWeakestEnemyToHeal].currentWarriorHealth < enemyArmy[lookingForWeakestEnemyToHeal].currentWarriorHealth && enemyArmy[lookingForWeakestEnemyToHeal].currentWarriorHealth > 0)
                                        {
                                            whichUnitToActUpon = lookingForWeakestEnemyToHeal;
                                        }
                                    }
                                }
                            }

                            //if the action chosen was attack then depending on whose turn it is, that index which was intered before will have received damage
                            if (choosingTurnAction == "A")
                            {
                                Random choosingWhoAttacks = new Random();
                                if (playerOneTurn == true)
                                {

                                    enemyArmy[whichUnitToActUpon].currentWarriorHealth -= allyArmy[choosingWhoAttacks.Next(0, allyCount)].createWarriorDamageStats(1);
                                }
                                else if (playerTwoTurn == true)
                                {

                                    allyArmy[whichUnitToActUpon].currentWarriorHealth -= enemyArmy[choosingWhoAttacks.Next(0, enemyCount)].createWarriorDamageStats(1);
                                }

                                //if the action chosen was heal then depending on whose turn it is, that index which was intered before will have healed hp
                            }
                            else if (choosingTurnAction == "H")
                            {
                                Random choosingWhoHeals = new Random();
                                if (playerOneTurn == true && allyArmy[whichUnitToActUpon].currentWarriorHealth > 0)
                                {
                                    allyArmy[whichUnitToActUpon].currentWarriorHealth += allyArmy[choosingWhoHeals.Next(0, allyCount)].createWarriorHealStats(1);
                                }
                                else if (playerTwoTurn == true && enemyArmy[whichUnitToActUpon].currentWarriorHealth > 0)
                                {
                                    enemyArmy[whichUnitToActUpon].currentWarriorHealth += enemyArmy[choosingWhoHeals.Next(0, enemyCount)].createWarriorHealStats(1);
                                }
                                else
                                {
                                    Console.WriteLine("Silly you cant heal dead units, too bad");
                                }
                            }
                        }
                        //if teh action type was defend, the player basically rolls a dice if they succesfully defended tthey get 2+ turns and if they did not they get 0+ turns and their defensive move was an epic failure >;]
                        else if (choosingTurnAction == "D" && defenceSuccessActedUpon == 0)
                        {

                            Random declaringDefencesuccess = new Random();
                            defenceSuccessActedUpon = (byte)declaringDefencesuccess.Next(0, 3);
                            while (defenceSuccessActedUpon == 1)
                            {
                                defenceSuccessActedUpon = (byte)declaringDefencesuccess.Next(0, 3);

                            }

                            if (defenceSuccessActedUpon == 0)
                            {

                                currentPlayerActionValidCheck = true;
                            }


                        }
                        Console.WriteLine(defenceSuccessActedUpon + " turn left for your round");

                    }

                    //if its was player1s turn its player 2s turn next, viceversa
                    if (playerOneTurn == true)
                    {
                        playerTwoTurn = true;
                        playerOneTurn = false;
                        Console.WriteLine("its their turn now");
                    }
                    else
                    {
                        playerTwoTurn = false;
                        playerOneTurn = true;
                        Console.WriteLine("It our turn now");
                    }
                    //checks how many <side>Army.currenthealth are dead in each side



                }

                return "draw";
            }























































































































































            //this function either returns draw or player1Won or player2Won
            static string makes2DTileForTicTacToe(byte yAxis, byte xAxis, string fillerOfGrid, string playerOneSymbolFiller, string playerTwoSymbolFiller)
            {
                if (fillerOfGrid == playerOneSymbolFiller || fillerOfGrid == playerTwoSymbolFiller || playerOneSymbolFiller == playerTwoSymbolFiller)
                {
                    fillerOfGrid = "-";
                    playerOneSymbolFiller = "x";
                    playerTwoSymbolFiller = "o";

                }
                bool playerOneTurn = false;
                bool playerTwoTurn = false;
                //short, int, byte are very similar the only diffrence is that byte holds 8 bits, short holds 16 bits & int holds 32 bits[long is not necessary here but itr can hold upto 64 bits]
                ///i did not dooo much on the variiable gameResultsPharse

                //this next variable is a counter which is invloved in counting if there are any empty slots left, and if there are none left the game ends and the function returns string variable of "draw"
                short succesfullyFoundEmptyFillerSlot = 0;
                bool gameEndCheck = false;
                //I initialise final Grid as {{-},{-},{-}} if xAxis=3
                string[][] finalGrid = new string[xAxis][];
                //I loop through each nested array basically the {-} in {{-}...}
                for (byte currentCycle = 0; currentCycle < xAxis; currentCycle += 1)
                {
                    finalGrid[currentCycle] = new string[yAxis];
                    for (byte specifiedNestedArrayCycle = 0; specifiedNestedArrayCycle < yAxis; specifiedNestedArrayCycle += 1)
                    {
                        finalGrid[currentCycle][specifiedNestedArrayCycle] = fillerOfGrid;
                    }
                }
                //this if statement only runs at te starts of the game ,becasue at the start its neither player's turn so here the randomizer chooses who will start first
                Random whichPlayerStartsDecision = new Random();
                if (playerOneTurn == false && playerTwoTurn == false)
                {
                    if (whichPlayerStartsDecision.Next(0, 2) == 0)
                    {
                        playerOneTurn = true;
                    }
                    else
                    {
                        playerTwoTurn = true;
                    }



                }
                //this whole while loop runs until the slots are  empty or a player has won{little note: i just made the the system which checks if slots are empty i did not make the system which checks if a player won
                string gameTypeDecision = "null";
                while (gameTypeDecision != "PvP" && gameTypeDecision != "PvAI")
                {
                    Console.WriteLine("enter PvP/n or PvAI");
                    gameTypeDecision = Console.ReadLine();
                }
                while (gameEndCheck == false)
                {
                    string currentPlayerValidFiller = "";
                    if (playerOneTurn == true)
                    {
                        playerOneTurn = false;
                        playerTwoTurn = true;
                        currentPlayerValidFiller = playerOneSymbolFiller;

                    }
                    else
                    {
                        playerOneTurn = true;
                        playerTwoTurn = false;
                        currentPlayerValidFiller = playerTwoSymbolFiller;
                    }
                    //asks user the coordinates of where they wanna put their symbol
                    short yCoordinateOfUserSign = -1;
                    short xCoordinateOfUserSign = -1;
                    //the boolean after this sentenc e is used as a pass which if true means that the turn was valid and if the pass is still false[maybe becuz the user put atooo big value or becasue they have selected a slot which is already occupied) it will ask the user to input the wanted coordinate of their mark
                    bool validTurnPassChecker = false;

                    if (gameTypeDecision == "PvP" || (playerOneTurn == false && gameTypeDecision == "PvAI"))
                    {
                        while (validTurnPassChecker == false)
                        {
                            //I have used a try catch block just in case the user puts something more than 16bits(cuz value type short only max allows foor 16bits)
                            try
                            {


                                Console.Write("Enter the X put ur sign");
                                xCoordinateOfUserSign = Convert.ToInt16(Console.ReadLine());
                                Console.Write("Enter the negative Y put ur sign");
                                yCoordinateOfUserSign = Convert.ToInt16(Console.ReadLine());
                            }
                            catch (Exception)
                            {

                                Console.WriteLine("An Error has occured , please check if you have put a valid value not too large or if you have useed anything else apart from allowed value byte type chars");
                            }
                            //the next iff statemnt checks if there the slected slot of the current user taking the turn is already filled or if the coordinate isnert5ed is invalid
                            if ((xCoordinateOfUserSign < 0 || yCoordinateOfUserSign < 0) || (xCoordinateOfUserSign > xAxis - 1 || yCoordinateOfUserSign > yAxis - 1) || !(finalGrid[yCoordinateOfUserSign][xCoordinateOfUserSign] == fillerOfGrid))
                            {
                                validTurnPassChecker = false;
                            }
                            else
                            {
                                validTurnPassChecker = true;
                            }

                        }
                    }
                    else
                    {

                        bool randomPlaceHasBeenFoundByAI = false;
                        for (byte lookingForXTargetAxis = 0; lookingForXTargetAxis < xAxis; lookingForXTargetAxis++)
                        {
                            for (byte lookingForYTargetAxis = 0; lookingForYTargetAxis < yAxis; lookingForYTargetAxis++)
                            {
                                //checks if there is any danger of a /n--X--/n-----/n----X by detceting that middle space and block it by /n--X--/n--X--/n----X
                                if (finalGrid[lookingForXTargetAxis][lookingForYTargetAxis] == playerOneSymbolFiller && lookingForXTargetAxis < xAxis - 2 && finalGrid[lookingForXTargetAxis + 2][lookingForYTargetAxis] == playerOneSymbolFiller)
                                {
                                    if (finalGrid[lookingForXTargetAxis + 1][lookingForYTargetAxis] == fillerOfGrid)
                                    {
                                        finalGrid[lookingForXTargetAxis + 1][lookingForYTargetAxis] = playerTwoSymbolFiller;
                                    }

                                }
                                else
                                {
                                    //makes random type value if there is no potential threat
                                    Random randomSlotEverythingCalm = new Random();
                                    while (randomPlaceHasBeenFoundByAI == false)
                                    {

                                        int randomXcoordByAI = randomSlotEverythingCalm.Next(0, xAxis);
                                        Console.WriteLine(randomXcoordByAI);
                                        int randomYcoordByAI = randomSlotEverythingCalm.Next(0, yAxis);
                                        Console.WriteLine(randomYcoordByAI);
                                        if (finalGrid[randomXcoordByAI][randomYcoordByAI] == fillerOfGrid && randomPlaceHasBeenFoundByAI == false)
                                        {
                                            finalGrid[randomXcoordByAI][randomYcoordByAI] = playerTwoSymbolFiller;
                                            randomPlaceHasBeenFoundByAI = true;
                                        }
                                    }


                                }
                            }
                        }
                    }

                    //if its playerOne's turn the fillerTypeOne will be used, if its player2's turn fillertype2 will get used
                    if (gameTypeDecision == "PvP" || (playerOneTurn == false && gameTypeDecision == "PvAI"))
                    {

                        finalGrid[yCoordinateOfUserSign][xCoordinateOfUserSign] = currentPlayerValidFiller;
                    }







                    //here  slects if a section of the xAxis of this tictactoe grid 2D array
                    for (byte checkingIfGameEndedXAxis = 0; checkingIfGameEndedXAxis < xAxis; checkingIfGameEndedXAxis++)
                    {

                        //the variable beneath this sentence accumulates the contenmts of each xAxis, basically takes all individual slots{with agiven coordinate  x,y} and puts them in a supposed longer string
                        string oneRowOfTicTacToeSlots = "";
                        //here it slectes the individual coordinate by slecting the YAxis of the XAxis row in this 2d tictactoe array
                        for (byte checkingIfGameEndedYAxis = 0; checkingIfGameEndedYAxis < yAxis; checkingIfGameEndedYAxis++)
                        {
                            oneRowOfTicTacToeSlots += finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis];
                            if ((finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == currentPlayerValidFiller) && (checkingIfGameEndedXAxis < xAxis - 2) && (checkingIfGameEndedYAxis < yAxis))
                            {
                                if ((finalGrid[checkingIfGameEndedXAxis + 1][checkingIfGameEndedYAxis] == currentPlayerValidFiller) && (finalGrid[checkingIfGameEndedXAxis + 2][checkingIfGameEndedYAxis] == currentPlayerValidFiller))
                                {
                                    Console.WriteLine("We found a vertical line of 3");
                                    gameEndCheck = true;
                                }
                                if ((finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == currentPlayerValidFiller) && (checkingIfGameEndedXAxis < xAxis - 2) && (checkingIfGameEndedYAxis > 1))
                                {
                                    if ((finalGrid[checkingIfGameEndedXAxis + 1][checkingIfGameEndedYAxis - 1] == currentPlayerValidFiller) && (finalGrid[checkingIfGameEndedXAxis + 2][checkingIfGameEndedYAxis - 2] == currentPlayerValidFiller))
                                    {
                                        Console.WriteLine("yo we found a positive gradient line of 3");
                                        gameEndCheck = true;
                                    }
                                }
                            }
                            if ((finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == currentPlayerValidFiller) && (checkingIfGameEndedXAxis < xAxis - 2) && (checkingIfGameEndedYAxis < yAxis - 2))
                            {
                                if ((finalGrid[checkingIfGameEndedXAxis + 1][checkingIfGameEndedYAxis + 1] == currentPlayerValidFiller) && (finalGrid[checkingIfGameEndedXAxis + 2][checkingIfGameEndedYAxis + 2] == currentPlayerValidFiller))
                                {
                                    Console.WriteLine("yo we found a negative gradient line of 3");
                                    gameEndCheck = true;
                                }

                            }
                            if ((finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == currentPlayerValidFiller) && (checkingIfGameEndedXAxis < xAxis) && (checkingIfGameEndedYAxis < yAxis - 2))
                            {
                                if ((finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis + 1] == currentPlayerValidFiller) && (finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis + 2] == currentPlayerValidFiller))
                                {
                                    Console.WriteLine("We found a horizontal line of 3");
                                    gameEndCheck = true;
                                }
                            }
                            //checks if a ticTacToe has been scored, the ticTacToe detectors are the previous 4 if statemnets Lol
                            if (gameEndCheck == true)
                            {
                                if (playerTwoTurn == false)
                                {
                                    return "player2Won";
                                }
                                else
                                {
                                    return "player1Won";
                                }
                            }



                            //checks if there is any filler every cycle in this for loop --> [checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == fillerOfGrid
                            if (finalGrid[checkingIfGameEndedXAxis][checkingIfGameEndedYAxis] == fillerOfGrid)
                            {
                                //if the fillerr was "-" and if this section of the nested loop finds a "-" in the finalGrid 2d array the suuccesfullyFoundEmptySlot gets incremented

                                succesfullyFoundEmptyFillerSlot++;
                            }
                        }
                        Console.WriteLine(oneRowOfTicTacToeSlots);
                    }
                    if (succesfullyFoundEmptyFillerSlot == 0)
                    {
                        gameEndCheck = true;
                        Console.WriteLine("Game Ended");
                    }
                    else
                    {
                        //tells u how many slots are left
                        Console.WriteLine(Convert.ToString(succesfullyFoundEmptyFillerSlot) + " empty filler slots left");
                        //it resets the amount of empty slots found cause the player will fill when the next cycle comes
                        succesfullyFoundEmptyFillerSlot = 0;
                    }

                }

                return "draw";
            }

        }

    }
}
