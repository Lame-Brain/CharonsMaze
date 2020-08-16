using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeObject : MonoBehaviour
{
    private Object[] mats, prefabs;    
    private int[,] floorTx, westTx, northTx, ceilingTx, maze;
    private GameObject[,] floorGo, westGo, northGo, ceilingGo;
    private const int cMazeSize = 31;

    void Start()
    {    
    }

    public void MakeMaze()
    {
        mats = Resources.LoadAll("Materials", typeof(Material));
        prefabs = Resources.LoadAll("Prefabs");
        GameObject wall = (GameObject)prefabs[3];
        GameObject door = (GameObject)prefabs[1];
        GameObject goldDoor = (GameObject)prefabs[5];
        GameObject floor = (GameObject)prefabs[2];
        GameObject ceiling = (GameObject)prefabs[0];
        floorTx = new int[cMazeSize, cMazeSize];
        ceilingTx = new int[cMazeSize, cMazeSize];
        westTx = new int[cMazeSize + 1, cMazeSize + 1];
        northTx = new int[cMazeSize + 1, cMazeSize + 1];
        maze = new int[cMazeSize, cMazeSize];
        floorGo = new GameObject[cMazeSize, cMazeSize];
        ceilingGo = new GameObject[cMazeSize, cMazeSize];
        westGo = new GameObject[cMazeSize + 1, cMazeSize + 1];
        northGo = new GameObject[cMazeSize + 1, cMazeSize + 1];

        //initialize Ceiling and Floor textures
        for (int y = 0; y < cMazeSize; y++)
        {
            for (int x = 0; x < cMazeSize; x++)
            {
                floorTx[x, y] = 5;
                ceilingTx[x, y] = 7;
                westTx[x, y] = 8;
                northTx[x, y] = 8;
            }
        }

        //initialize Wall textures
        for (int i = 0; i <= cMazeSize; i++)
        {
            northTx[i, 0] = 8;
            northTx[i, cMazeSize] = 8;
            westTx[0, i] = 8;
            westTx[cMazeSize, i] = 8;
        }

        //Generate Maze Structure
        for (int y = 0; y < cMazeSize; y++) { for (int x = 0; x < cMazeSize; x++) { maze[x, y] = 0; } } //Clear the array

        //Generate corridor around perimieter
        for (int i = 0; i < cMazeSize; i++)
        {
            maze[i, 0] = 2;
            maze[i, cMazeSize - 1] = 2;
            maze[0, i] = 2;
            maze[cMazeSize - 1, i] = 2;
        }
        for (int i = 1; i < cMazeSize; i++)
        {
            westTx[i, 0] = 0;
            westTx[i, cMazeSize - 1] = 0;
            northTx[0, i] = 0;
            northTx[cMazeSize - 1, i] = 0;
        }

        //Generate Maze
        //1. Seed a bunch of cells that are not -1 or 2 to be 1
        //2. Scan for cell marked 1
        //3. Generate corridors
        //4. if you cannot find any cells marked 1, scan for any remaining cells marked 0 and mark them 1 and go back to step 2
        //5. if you cannot find any cells marked 1, and you cannot find any cells marked 0, then done
        int rndmx = 5, rndmy = 5, selectedX = 0, selectedY = 0, moveX, moveY, randomDirection;
        bool loopDone, foundCellMarked1, foundCellMarked0, mazeCompleted, pickedDirection;

        //Step 1: Seed a bunch of cells that are not -1 or 2 to be 1
        for (int i = 0; i < 20; i++) 
        {
            loopDone = false;
            while (!loopDone)
            {
                rndmx = Random.Range(2, cMazeSize - 3); rndmy = Random.Range(2, cMazeSize - 3);
                if (maze[rndmx, rndmy] == 0) loopDone = true;
            }
            Debug.Log("-> X = " + rndmx + ", Y = " + rndmy);
            maze[rndmx, rndmy] = 1;
        }


        mazeCompleted = false;
        while (!mazeCompleted)
        {
            //2. Scan for cell marked 1
            foundCellMarked1 = false;
            for (int y = 0; y < cMazeSize; y++)
            {
                for (int x = 0; x < cMazeSize; x++)
                {
                    if (maze[x, y] == 1) { selectedX = x; selectedY = y; foundCellMarked1 = true; }
                }
            }
            //3. Generate corridors
            randomDirection = Random.Range(0, 3); //generate direction
            pickedDirection = false; moveX = 0; moveY = 0; //initialize variables
            for (int i = 0; i < 4; i++)
            {   //set moveX and moveY based on chosen direction
                if (randomDirection == 0) { moveX = 0; moveY = -1; } //face north
                if (randomDirection == 1) { moveX = 1; moveY = 0; } //face east
                if (randomDirection == 2) { moveX = 0; moveY = 1; } //face south
                if (randomDirection == 3) { moveX = -1; moveY = 0; } //face west
                if (maze[selectedX + moveX, selectedY + moveY] == -1)
                {
                    randomDirection++;
                    if (randomDirection > 3) randomDirection = 0;
                }
                else { pickedDirection = true; }
            }
            if (!pickedDirection) maze[selectedX, selectedY] = 2; //No direction is valid, mark cell as 2 and move on
            if (pickedDirection) //this triggers if a direction was chosen
            {
                if (randomDirection == 0) northTx[selectedX, selectedY] = 0;
                if (randomDirection == 1) westTx[selectedX + 1, selectedY] = 0;
                if (randomDirection == 2) northTx[selectedX, selectedY + 1] = 0;
                if (randomDirection == 3) westTx[selectedX, selectedY] = 0;
                maze[selectedX, selectedY] = 2;

                if (maze[selectedX + moveX, selectedY + moveY] == 0)  maze[selectedX + moveX, selectedY + moveY] = 1;
                if (maze[selectedX + moveX, selectedY + moveY] == 2 || maze[selectedX + moveX, selectedY + moveY] == 1)
                {
                    if (randomDirection == 0) northTx[selectedX + moveX, selectedY + moveY] = 0;
                    if (randomDirection == 1) westTx[selectedX + moveX + 1, selectedY + moveY] = 0;
                    if (randomDirection == 2) northTx[selectedX + moveX, selectedY + moveY + 1] = 0;
                    if (randomDirection == 3) westTx[selectedX + moveX, selectedY + moveY] = 0;
                }
            }
            //4. if you cannot find any cells marked 1, scan for any remaining cells marked 0 and mark them 1 and go back to step 2
            foundCellMarked0 = false;
            if (!foundCellMarked1)
            {
                for (int y = 0; y < cMazeSize; y++)
                {
                    for (int x = 0; x < cMazeSize; x++)
                    {
                        if (maze[x, y] == 0) { maze[x, y] = 1; foundCellMarked0 = true; }                                
                    }
                }
            }

            //5. if you cannot find any cells marked 1, and you cannot find any cells marked 0, then done
            if (!foundCellMarked0 && !foundCellMarked1) mazeCompleted = true;

        }
        //Generate walls around perimeter
        for(int i = 0; i < cMazeSize; i++)
        {
            northTx[i, 0] = 8;
            northTx[i, cMazeSize] = 8;
            westTx[0, i] = 8;
            westTx[cMazeSize, i] = 8;
        }

        //Generate Exit Room
        int rx = 0, ry = 0;
        rx = Random.Range(0, cMazeSize - 6); ry = 0; //find a random location on top row for exit
        for (int y = 0; y < 5; y++) { for (int x = rx; x < rx + 5; x++) { maze[x, y] = -1; } } //mark room as filled
        maze[rx + 2, 5] = 1;//mark exit of room as space to generate hallway
        northTx[rx, 1] = 0; northTx[rx + 1, 1] = 0; northTx[rx + 2, 1] = 0; northTx[rx + 3, 1] = 0; northTx[rx + 4, 1] = 0; //internal spaces
        northTx[rx, 2] = 0; northTx[rx + 1, 2] = 0; northTx[rx + 2, 2] = 0; northTx[rx + 3, 2] = 0; northTx[rx + 4, 2] = 0;
        northTx[rx, 3] = 0; northTx[rx + 1, 3] = 0; northTx[rx + 2, 3] = 0; northTx[rx + 3, 3] = 0; northTx[rx + 4, 3] = 0;
        northTx[rx, 4] = 0; northTx[rx + 1, 4] = 0; northTx[rx + 2, 4] = 0; northTx[rx + 3, 4] = 0; northTx[rx + 4, 4] = 0;
        westTx[rx + 1, 0] = 0; westTx[rx + 2, 0] = 0; westTx[rx + 3, 0] = 0; westTx[rx + 3, 0] = 0; westTx[rx + 4, 0] = 0;
        westTx[rx + 1, 1] = 0; westTx[rx + 2, 1] = 0; westTx[rx + 3, 1] = 0; westTx[rx + 3, 1] = 0; westTx[rx + 4, 1] = 0;
        westTx[rx + 1, 2] = 0; westTx[rx + 2, 2] = 0; westTx[rx + 3, 2] = 0; westTx[rx + 3, 2] = 0; westTx[rx + 4, 2] = 0;
        westTx[rx + 1, 3] = 0; westTx[rx + 2, 3] = 0; westTx[rx + 3, 3] = 0; westTx[rx + 3, 3] = 0; westTx[rx + 4, 3] = 0;
        westTx[rx + 1, 4] = 0; westTx[rx + 2, 4] = 0; westTx[rx + 3, 4] = 0; westTx[rx + 3, 4] = 0; westTx[rx + 4, 4] = 0;
        northTx[rx, 0] = 8; northTx[rx+1, 0] = 8; northTx[rx+2, 0] = 13; northTx[rx+3, 0] = 8; northTx[rx+4, 0] = 8; //Walls
        northTx[rx, 5] = 8; northTx[rx + 1, 5] = 8; northTx[rx + 2, 5] = 3; northTx[rx + 3, 5] = 8; northTx[rx + 4, 5] = 8;
        westTx[rx, 0] = 8; westTx[rx + 5, 0] = 8;
        westTx[rx, 1] = 8; westTx[rx + 5, 1] = 8;
        westTx[rx, 2] = 8; westTx[rx + 5, 2] = 8;
        westTx[rx, 3] = 8; westTx[rx + 5, 3] = 8;
        westTx[rx, 4] = 8; westTx[rx + 5, 4] = 8;
        floorTx[rx + 1, ry + 1] = 11; floorTx[rx + 3, ry + 1] = 11; //Pillars
        floorTx[rx + 1, ry + 3] = 11; floorTx[rx + 3, ry + 3] = 11;
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3((rx + 2) * 2, 0, (ry + 1) * 2); //place the player
        GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.identity; //Rotate the player


        //Generate Entry Room
        bool done = false;
        while (!done)
        {
            rx = Random.Range(2, cMazeSize - 4); ry = Random.Range(0, cMazeSize - 4); //find a random location on entry room
            done = true;
            for (int y = ry; y < ry+4; y++) { for (int x = rx; x < rx + 4; x++) { if (maze[x, y] == -1) done = false; } } //scan to make sure there is room for this room
        }
        for (int y = ry; y < ry + 3; y++) { for (int x = rx; x < rx + 3; x++) { maze[x, y] = -1; } } // set room into maze array
        //maze[rx + 1, ry + 3] = 1; //mark exit of room to generate hallway        
        northTx[rx, ry + 1] = 0; northTx[rx + 1, ry + 1] = 0; northTx[rx + 2, ry + 1] = 0;//internal spaces
        northTx[rx, ry + 2] = 0; northTx[rx + 1, ry + 2] = 0; northTx[rx + 2, ry + 2] = 0;
        westTx[rx + 1, ry] = 0; westTx[rx + 2, ry] = 0;
        westTx[rx + 1, ry+1] = 0; westTx[rx + 2, ry+1] = 0;
        westTx[rx + 1, ry+2] = 0; westTx[rx + 2, ry+2] = 0;
        northTx[rx, ry] = 8; northTx[rx+1, ry] = 3; northTx[rx+2, ry] = 8; //walls
        northTx[rx, ry + 3] = 8; northTx[rx + 1, ry + 3] = 3; northTx[rx + 2, ry + 3] = 8;
        westTx[rx, ry] = 8; westTx[rx + 3, ry] = 8;
        westTx[rx, ry+1] = 3; westTx[rx + 3, ry+1] = 3; 
        westTx[rx, ry+2] = 8; westTx[rx + 3, ry+2] = 8;
        floorTx[rx, ry] = 11; floorTx[rx + 2, ry] = 11; //Pillars
        floorTx[rx, ry+2] = 11; floorTx[rx + 2, ry+2] = 11;        
        Instantiate(prefabs[10], new Vector3((rx + 1) * 2, 0, (ry + 1) * 2), Quaternion.identity);

        //Generate key1 room
        done = false;
        while (!done)
        {
            rx = Random.Range(1, cMazeSize - 7); ry = Random.Range(1, cMazeSize - 7); //find a random location for room
            done = true;
            for (int y = ry; y < ry + 7; y++) { for (int x = rx; x < rx + 7; x++) { if (maze[x, y] == -1) done = false; } } //scan to make sure there is room for this room
        }
        for (int y = ry; y < ry + 6; y++) { for (int x = rx; x < rx + 6; x++) { maze[x, y] = -1; } } // set room into maze array
        for (int y = ry; y < ry + 3; y++) { for (int x = rx + 3; x < rx + 6; x++) { maze[x, y] = 0; } } // put some back
        maze[rx + 3, ry + 1] = 1; //mark exit of room to generate hallway
        northTx[rx, ry + 1] = 0; northTx[rx + 1, ry + 1] = 0; northTx[rx + 2, ry + 1] = 0; //internal spaces
        northTx[rx, ry + 2] = 0; northTx[rx + 1, ry + 2] = 0; northTx[rx + 2, ry + 2] = 0;
        northTx[rx, ry + 3] = 0; northTx[rx + 1, ry + 3] = 0; northTx[rx + 2, ry + 3] = 0;
        northTx[rx, ry + 4] = 0; northTx[rx + 1, ry + 4] = 0; northTx[rx + 2, ry + 4] = 0;
        northTx[rx, ry + 5] = 0; northTx[rx + 1, ry + 5] = 0; northTx[rx + 2, ry + 5] = 0;
        northTx[rx + 3, ry + 4] = 0; northTx[rx + 4, ry + 4] = 0; northTx[rx + 5, ry + 4] = 0;
        northTx[rx + 3, ry + 5] = 0; northTx[rx + 4, ry + 5] = 0; northTx[rx + 5, ry + 5] = 0;
        westTx[rx + 1, ry] = 0;     westTx[rx + 2, ry] = 0;
        westTx[rx + 1, ry + 1] = 0; westTx[rx + 2, ry + 1] = 0; 
        westTx[rx + 1, ry + 2] = 0; westTx[rx + 2, ry + 2] = 0;
        westTx[rx + 1, ry + 3] = 0; westTx[rx + 2, ry + 3] = 0; 
        westTx[rx + 1, ry + 4] = 0; westTx[rx + 2, ry + 4] = 0; 
        westTx[rx + 1, ry + 5] = 0; westTx[rx + 2, ry + 5] = 0;
        westTx[rx + 3, ry + 3] = 0; westTx[rx + 4, ry + 3] = 0; westTx[rx + 5, ry + 3] = 0;
        westTx[rx + 3, ry + 4] = 0; westTx[rx + 4, ry + 4] = 0; westTx[rx + 5, ry + 4] = 0;
        westTx[rx + 3, ry + 5] = 0; westTx[rx + 4, ry + 5] = 0; westTx[rx + 5, ry + 5] = 0;
        northTx[rx, ry] = 8; northTx[rx + 1, ry] = 3; northTx[rx + 2, ry] = 8; //Walls
        northTx[rx + 3, ry + 3] = 8; northTx[rx + 4, ry + 3] = 8; northTx[rx + 5, ry + 3] = 8;
        northTx[rx, ry + 6] = 8; northTx[rx + 1, ry + 6] = 8; northTx[rx + 2, ry + 6] = 8; northTx[rx + 3, ry + 6] = 8; northTx[rx + 4, ry + 6] = 8; northTx[rx + 5, ry + 6] = 8;
        westTx[rx, ry] = 8; westTx[rx + 3, ry] = 8;
        westTx[rx, ry + 1] = 8; westTx[rx + 3, ry + 1] = 8;
        westTx[rx, ry + 2] = 8; westTx[rx + 3, ry + 2] = 8;
        westTx[rx, ry + 3] = 8; westTx[rx + 6, ry + 3] = 8;
        westTx[rx, ry + 4] = 8; westTx[rx + 6, ry + 4] = 3;
        westTx[rx, ry + 5] = 8; westTx[rx + 6, ry + 5] = 8; 
        floorTx[rx, ry] = 11; floorTx[rx + 2, ry] = 11; //Pillars
        floorTx[rx, ry+5] = 11; floorTx[rx + 5, ry + 3] = 11; floorTx[rx + 5, ry + 5] = 11;
        Instantiate(prefabs[6], new Vector3((rx + 1) * 2, 0, (ry + 4) * 2), Quaternion.identity);        


        //Generate key2 room
        done = false;
        while (!done)
        {
            rx = Random.Range(1, cMazeSize - 7); ry = Random.Range(1, cMazeSize - 7); //find a random location for room
            done = true;
            for (int y = ry; y < ry + 7; y++) { for (int x = rx; x < rx + 7; x++) { if (maze[x, y] == -1) done = false; } } //scan to make sure there is room for this room
        }
        for (int y = ry; y < ry + 6; y++) { for (int x = rx; x < rx + 6; x++) { maze[x, y] = -1; } } // set room into maze array
        for (int y = ry; y < ry + 3; y++) { for (int x = rx; x < rx + 3; x++) { maze[x, y] = 0; } } // put some back
        maze[rx + 2, ry + 1] = 1; //mark exit of room to generate hallway
        northTx[rx + 3, ry + 1] = 0; northTx[rx + 4, ry + 1] = 0; northTx[rx + 5, ry + 1] = 0; //internal spaces
        northTx[rx + 3, ry + 2] = 0; northTx[rx + 4, ry + 2] = 0; northTx[rx + 5, ry + 2] = 0;
        northTx[rx, ry + 4] = 0; northTx[rx + 1, ry + 4] = 0; northTx[rx + 2, ry + 4] = 0;
        northTx[rx, ry + 5] = 0; northTx[rx + 1, ry + 5] = 0; northTx[rx + 2, ry + 5] = 0;
        northTx[rx + 3, ry + 3] = 0; northTx[rx + 4, ry + 3] = 0; northTx[rx + 5, ry + 3] = 0;
        northTx[rx + 3, ry + 4] = 0; northTx[rx + 4, ry + 4] = 0; northTx[rx + 5, ry + 4] = 0;
        northTx[rx + 3, ry + 5] = 0; northTx[rx + 4, ry + 5] = 0; northTx[rx + 5, ry + 5] = 0;
        westTx[rx + 4, ry] = 0;     westTx[rx + 5, ry] = 0;
        westTx[rx + 4, ry + 1] = 0; westTx[rx + 5, ry + 1] = 0;
        westTx[rx + 4, ry + 2] = 0; westTx[rx + 5, ry + 2] = 0;
        westTx[rx + 1, ry + 3] = 0; westTx[rx + 2, ry + 3] = 0;
        westTx[rx + 1, ry + 4] = 0; westTx[rx + 2, ry + 4] = 0;
        westTx[rx + 1, ry + 5] = 0; westTx[rx + 2, ry + 5] = 0;
        westTx[rx + 3, ry + 3] = 0; westTx[rx + 4, ry + 3] = 0; westTx[rx + 5, ry + 3] = 0;
        westTx[rx + 3, ry + 4] = 0; westTx[rx + 4, ry + 4] = 0; westTx[rx + 5, ry + 4] = 0;
        westTx[rx + 3, ry + 5] = 0; westTx[rx + 4, ry + 5] = 0; westTx[rx + 5, ry + 5] = 0;
        northTx[rx + 3, ry] = 8; northTx[rx + 4, ry] = 3; northTx[rx + 5, ry] = 8; //Walls
        northTx[rx, ry + 3] = 8; northTx[rx + 1, ry + 3] = 8; northTx[rx + 2, ry + 3] = 8;
        northTx[rx, ry + 6] = 8; northTx[rx + 1, ry + 6] = 8; northTx[rx + 2, ry + 6] = 8; northTx[rx + 3, ry + 6] = 8; northTx[rx + 4, ry + 6] = 8; northTx[rx + 5, ry + 6] = 8;
        westTx[rx + 3, ry] = 8; westTx[rx + 6, ry] = 8;
        westTx[rx + 3, ry + 1] = 8; westTx[rx + 6, ry + 1] = 8;
        westTx[rx + 3, ry + 2] = 8; westTx[rx + 6, ry + 2] = 8;
        westTx[rx, ry + 3] = 8; westTx[rx + 6, ry + 3] = 8;
        westTx[rx, ry + 4] = 3; westTx[rx + 6, ry + 4] = 8;
        westTx[rx, ry + 5] = 8; westTx[rx + 6, ry + 5] = 8;
        floorTx[rx + 3, ry] = 11; floorTx[rx + 5, ry] = 11; //Pillars
        floorTx[rx + 5, ry + 5] = 11; floorTx[rx, ry + 3] = 11; floorTx[rx, ry + 5] = 11;
        Instantiate(prefabs[7], new Vector3((rx + 4) * 2, 0, (ry + 4) * 2), Quaternion.identity);

        //Generate key3 room
        done = false;
        while (!done)
        {
            rx = Random.Range(1, cMazeSize - 7); ry = Random.Range(1, cMazeSize - 7); //find a random location for room
            done = true;
            for (int y = ry; y < ry + 7; y++) { for (int x = rx; x < rx + 7; x++) { if (maze[x, y] == -1) done = false; } } //scan to make sure there is room for this room
        }
        for (int y = ry; y < ry + 6; y++) { for (int x = rx; x < rx + 6; x++) { maze[x, y] = -1; } } // set room into maze array
        for (int y = ry + 3; y < ry + 6; y++) { for (int x = rx + 3; x < rx + 6; x++) { maze[x, y] = 0; } } // put some back
        maze[rx + 3, ry + 4] = 1; //mark exit of room to generate hallway
        northTx[rx, ry + 1] = 0; northTx[rx + 1, ry + 1] = 0; northTx[rx + 2, ry + 1] = 0; //internal spaces
        northTx[rx, ry + 2] = 0; northTx[rx + 1, ry + 2] = 0; northTx[rx + 2, ry + 2] = 0;
        northTx[rx + 3, ry + 1] = 0; northTx[rx + 4, ry + 1] = 0; northTx[rx + 5, ry + 1] = 0;
        northTx[rx + 3, ry + 2] = 0; northTx[rx + 4, ry + 2] = 0; northTx[rx + 5, ry + 2] = 0;
        northTx[rx, ry + 3] = 0; northTx[rx + 1, ry + 3] = 0; northTx[rx + 2, ry + 3] = 0;
        northTx[rx, ry + 4] = 0; northTx[rx + 1, ry + 4] = 0; northTx[rx + 2, ry + 4] = 0;
        northTx[rx, ry + 5] = 0; northTx[rx + 1, ry + 5] = 0; northTx[rx + 2, ry + 5] = 0;
        westTx[rx + 1, ry] = 0;     westTx[rx + 2, ry] = 0;     westTx[rx + 3, ry] = 0;     westTx[rx + 4, ry] = 0;     westTx[rx + 5, ry] = 0;
        westTx[rx + 1, ry + 1] = 0; westTx[rx + 2, ry + 1] = 0; westTx[rx + 3, ry + 1] = 0; westTx[rx + 4, ry + 1] = 0; westTx[rx + 5, ry + 1] = 0;
        westTx[rx + 1, ry + 2] = 0; westTx[rx + 2, ry + 2] = 0; westTx[rx + 3, ry + 2] = 0; westTx[rx + 4, ry + 2] = 0; westTx[rx + 5, ry + 2] = 0;
        westTx[rx + 1, ry + 3] = 0; westTx[rx + 2, ry + 3] = 0;
        westTx[rx + 1, ry + 4] = 0; westTx[rx + 2, ry + 4] = 0; westTx[rx + 3, ry + 4] = 3;
        westTx[rx + 1, ry + 5] = 0; westTx[rx + 2, ry + 5] = 0;
        northTx[rx, ry] = 8; northTx[rx + 1, ry] = 8; northTx[rx + 2, ry] = 8; northTx[rx + 3, ry] = 8; northTx[rx + 4, ry] = 8; northTx[rx + 5, ry] = 8; //Walls
        northTx[rx + 3, ry + 3] = 8; northTx[rx + 4, ry + 3] = 8; northTx[rx + 5, ry + 3] = 8;
        northTx[rx, ry + 6] = 8; northTx[rx + 1, ry + 6] = 3; northTx[rx + 2, ry + 6] = 8;
        westTx[rx, ry] = 8; westTx[rx + 6, ry] = 8;
        westTx[rx, ry + 1] = 8; westTx[rx + 6, ry + 1] = 3;
        westTx[rx, ry + 2] = 8; westTx[rx + 6, ry + 2] = 8;
        westTx[rx, ry + 3] = 8; westTx[rx + 3, ry + 3] = 8;
        westTx[rx, ry + 4] = 8; westTx[rx + 3, ry + 4] = 8;
        westTx[rx, ry + 5] = 8; westTx[rx + 3, ry + 5] = 8;
        floorTx[rx, ry] = 11; floorTx[rx + 5, ry] = 11; //Pillars
        floorTx[rx + 5, ry + 2] = 11; floorTx[rx, ry + 5] = 11; floorTx[rx + 2, ry + 5] = 11;
        Instantiate(prefabs[8], new Vector3((rx + 1) * 2, 0, (ry + 1) * 2), Quaternion.identity);


        //Generate key4 room
        done = false;
        while (!done)
        {
            rx = Random.Range(1, cMazeSize - 7); ry = Random.Range(1, cMazeSize - 7); //find a random location for room
            done = true;
            for (int y = ry; y < ry + 7; y++) { for (int x = rx; x < rx + 7; x++) { if (maze[x, y] == -1) done = false; } } //scan to make sure there is room for this room
        }
        for (int y = ry; y < ry + 6; y++) { for (int x = rx; x < rx + 6; x++) { maze[x, y] = -1; } } // set room into maze array
        for (int y = ry + 3; y < ry + 6; y++) { for (int x = rx; x < rx + 3; x++) { maze[x, y] = 0; } } // put some back
        maze[rx + 2, ry + 4] = 1; //mark exit of room to generate hallway
        northTx[rx, ry + 1] = 0; northTx[rx + 1, ry + 1] = 0; northTx[rx + 2, ry + 1] = 0; //internal spaces
        northTx[rx, ry + 2] = 0; northTx[rx + 1, ry + 2] = 0; northTx[rx + 2, ry + 2] = 0;
        northTx[rx + 3, ry + 1] = 0; northTx[rx + 4, ry + 1] = 0; northTx[rx + 5, ry + 1] = 0;
        northTx[rx + 3, ry + 2] = 0; northTx[rx + 4, ry + 2] = 0; northTx[rx + 5, ry + 2] = 0;
        northTx[rx + 3, ry + 3] = 0; northTx[rx + 4, ry + 3] = 0; northTx[rx + 5, ry + 3] = 0;
        northTx[rx + 3, ry + 4] = 0; northTx[rx + 4, ry + 4] = 0; northTx[rx + 5, ry + 4] = 0;
        northTx[rx + 3, ry + 5] = 0; northTx[rx + 4, ry + 5] = 0; northTx[rx + 5, ry + 5] = 0;
        westTx[rx + 1, ry] = 0;     westTx[rx + 2, ry] = 0;     westTx[rx + 3, ry] = 0;     westTx[rx + 4, ry] = 0;     westTx[rx + 5, ry] = 0;
        westTx[rx + 1, ry + 1] = 0; westTx[rx + 2, ry + 1] = 0; westTx[rx + 3, ry + 1] = 0; westTx[rx + 4, ry + 1] = 0; westTx[rx + 5, ry + 1] = 0;
        westTx[rx + 1, ry + 2] = 0; westTx[rx + 2, ry + 2] = 0; westTx[rx + 3, ry + 2] = 0; westTx[rx + 4, ry + 2] = 0; westTx[rx + 5, ry + 2] = 0;
        westTx[rx + 4, ry + 3] = 0; westTx[rx + 5, ry + 3] = 0;
        westTx[rx + 4, ry + 4] = 0; westTx[rx + 5, ry + 4] = 0; westTx[rx + 3, ry + 4] = 3;
        westTx[rx + 4, ry + 5] = 0; westTx[rx + 5, ry + 5] = 0;
        northTx[rx, ry] = 8; northTx[rx + 1, ry] = 8; northTx[rx + 2, ry] = 8; northTx[rx + 3, ry] = 8; northTx[rx + 4, ry] = 8; northTx[rx + 5, ry] = 8; //Walls
        northTx[rx, ry + 3] = 8; northTx[rx + 1, ry + 3] = 8; northTx[rx + 2, ry + 3] = 8;
        northTx[rx + 3, ry + 6] = 8; northTx[rx + 4, ry + 6] = 3; northTx[rx + 5, ry + 6] = 8;
        westTx[rx, ry] = 8; westTx[rx + 6, ry] = 8;
        westTx[rx, ry + 1] = 3; westTx[rx + 6, ry + 1] = 8;
        westTx[rx, ry + 2] = 8; westTx[rx + 6, ry + 2] = 8;
        westTx[rx + 3, ry + 3] = 8; westTx[rx + 6, ry + 3] = 8;
        westTx[rx + 3, ry + 4] = 8; westTx[rx + 6, ry + 4] = 8;
        westTx[rx + 3, ry + 5] = 8; westTx[rx + 6, ry + 5] = 8;
        floorTx[rx, ry] = 11; floorTx[rx + 5, ry] = 11; //Pillars
        floorTx[rx, ry + 2] = 11; floorTx[rx + 3, ry + 5] = 11; floorTx[rx + 5, ry + 5] = 11;
        Instantiate(prefabs[9], new Vector3((rx + 4) * 2, 0, (ry + 1) * 2), Quaternion.identity);

        //Randomize tiles
        int txtr = 0, xN, yN, xW, yW;
        for(int t = 1; t < 9; t++)
        {
            if (t == 1) txtr = 1;
            if (t == 2) txtr = 2;
            if (t == 3) txtr = 5;
            if (t == 4) txtr = 6;
            if (t == 5) txtr = 7;
            if (t == 6) txtr = 8;
            if (t == 7) txtr = 9;
            if (t == 8) txtr = 10;
            for (int i = 0; i < Random.Range(5, 25); i++)
            {
                xN = Random.Range(0, cMazeSize - 1); yN = Random.Range(0, cMazeSize - 1);
                xW = Random.Range(0, cMazeSize - 1); yW = Random.Range(0, cMazeSize - 1);
                if (westTx[xW, yW] != 0 && westTx[xW, yW] != 3 && westTx[xW, yW] != 13) westTx[xW, yW] = txtr;
                if (northTx[xN, yN] != 0 && northTx[xN, yN] != 3 && northTx[xN, yN] != 13) northTx[xN, yN] = txtr;
                ceilingTx[Random.Range(0, cMazeSize - 1), Random.Range(0, cMazeSize - 1)] = txtr;
                floorTx[Random.Range(0, cMazeSize - 1), Random.Range(0, cMazeSize - 1)] = txtr;
            }
        }

        //Destroy the maze
        GameObject[] MazeObjects = GameObject.FindGameObjectsWithTag("Maze");
        for (int i = 0; i < MazeObjects.Length; i++) Destroy(MazeObjects[i]);

        //Draw the maze
        for (int y = 0; y < cMazeSize; y++)
        {
            for(int x = 0; x < cMazeSize; x++)
            {
                if (floorTx[x, y] > 0)
                {                    
                    floorGo[x, y] = Instantiate(floor, new Vector3(x * 2, floor.transform.position.y, y * 2), floor.transform.rotation);
                    floorGo[x, y].GetComponent<MeshRenderer>().material = (Material)mats[floorTx[x, y]];
                    if (floorTx[x, y] == 11) { Instantiate(prefabs[4], new Vector3(x * 2, 0, y * 2), Quaternion.identity); floorGo[x, y].GetComponent<MeshRenderer>().material = (Material)mats[6]; }
                }
                if (ceilingTx[x, y] > 0)
                {
                    ceilingGo[x, y] = Instantiate(ceiling, new Vector3(x * 2, ceiling.transform.position.y, y * 2), ceiling.transform.rotation);
                    ceilingGo[x, y].GetComponent<MeshRenderer>().material = (Material)mats[ceilingTx[x, y]];
                }
                if(northTx[x,y] > 0)
                {
                    if(northTx[x,y] == 3 || northTx[x, y] == 13)
                    {
                        if(northTx[x, y] == 3) northGo[x, y] = Instantiate(door, new Vector3((x * 2), 0, (y * 2) - 1), Quaternion.Euler(0, 0, 0));
                        if (northTx[x, y] == 13) northGo[x, y] = Instantiate(goldDoor, new Vector3((x * 2), 0, (y * 2) - 1), Quaternion.Euler(0, 0, 0));
                    }
                    else {
                        northGo[x, y] = Instantiate(wall, new Vector3((x * 2), 0, (y * 2) - 1), Quaternion.Euler(0, 0, 0));
                        northGo[x, y].GetComponent<MeshRenderer>().material = (Material)mats[northTx[x, y]];
                    }
                }
                if (westTx[x, y] > 0)
                {
                    if (westTx[x, y] == 3 || westTx[x, y] == 13)
                    {
                        if (westTx[x, y] == 3) westGo[x, y] = Instantiate(door, new Vector3((x * 2) - 1, 0, (y * 2)), Quaternion.Euler(0, 90, 0));
                        if (westTx[x, y] == 13) westGo[x, y] = Instantiate(goldDoor, new Vector3((x * 2) - 1, 0, (y * 2)), Quaternion.Euler(0, 90, 0));

                    }
                    else
                    {
                        westGo[x, y] = Instantiate(wall, new Vector3((x * 2) - 1, 0, (y * 2)), Quaternion.Euler(0, 90, 0));
                        westGo[x, y].GetComponent<MeshRenderer>().material = (Material)mats[westTx[x, y]];
                    }
                }
            }
        }
        //draw the farsides
        for(int i = 0; i <cMazeSize; i++)
        {
            if (northTx[i, cMazeSize] > 0)
            {
                northGo[i, cMazeSize] = Instantiate(wall, new Vector3((i * 2), 0, (cMazeSize * 2) - 1), Quaternion.Euler(0, 0, 0));
                northGo[i, cMazeSize].GetComponent<MeshRenderer>().material = (Material)mats[northTx[i, cMazeSize]];
            }
            if (westTx[cMazeSize,i] > 0)
            {
                westGo[cMazeSize,i] = Instantiate(wall, new Vector3((cMazeSize * 2) - 1, 0, (i * 2)), Quaternion.Euler(0, 90, 0));
                westGo[cMazeSize,i].GetComponent<MeshRenderer>().material = (Material)mats[westTx[cMazeSize,i]];
            }
        }
    }
}

