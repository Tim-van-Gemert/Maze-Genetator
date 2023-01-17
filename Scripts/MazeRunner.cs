using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRunner : MonoBehaviour
{   
    public GameObject mazeGenerator;
    private MazeGenerator mazeGeneratorScript;
    private Coroutine lastMazeRunner = null;


    private int mazeSizeX;
    private int mazeSizeY;

    private int wallHeight;
    private bool fastMode;

    List<MazeCell> cells;
    List<MazeCell> currentPath = new List<MazeCell>();
    List<MazeCell> completedCells;


    public void EnableRunners()
    {
        //Stops previous Maze Runner from running
        if (lastMazeRunner != null)
        {
            StopCoroutine(lastMazeRunner); 
        }

        //Definies or resets the values of local variables 
        mazeGenerator = GameObject.Find("Maze Generator");
        mazeGeneratorScript = mazeGenerator.GetComponent<MazeGenerator>();

        currentPath = mazeGeneratorScript.currentPath;

        cells = mazeGeneratorScript.cells;
        mazeSizeX = mazeGeneratorScript.mazeSizeX;
        mazeSizeY = mazeGeneratorScript.mazeSizeY;

        fastMode = mazeGeneratorScript.fastMode;
        wallHeight = mazeGeneratorScript.wallHeight;

        //Starts Maze Runner
        lastMazeRunner = StartCoroutine(StartMazeRunner(mazeSizeX, mazeSizeY));
    }


    IEnumerator StartMazeRunner(int sizeX, int sizeY)
    {
        completedCells = mazeGeneratorScript.completedCells; 
        
        //Choose starting cell;
        currentPath.Add(cells[Random.Range(0, cells.Count)]);
        currentPath[0].SetState(CellState.Completed);


        while (completedCells.Count < cells.Count)
        {
            //Check neighbor cells
            List<int> possibleNextCells = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentCellIndex = cells.IndexOf(currentPath[currentPath.Count - 1]);
            int currentCellX = currentCellIndex / sizeY;
            int currentCellY = currentCellIndex % sizeY;


            //Checks if right neighbor is available
            if (currentCellX < sizeX -1)
            {
                if (!completedCells.Contains(cells[currentCellIndex + sizeY]) &&
                    !currentPath.Contains(cells[currentCellIndex + sizeY]))
                {
                    possibleDirections.Add(1);
                    possibleNextCells.Add(currentCellIndex + sizeY);

                }
            }

            //Checks if left neighbor is available
            if (currentCellX > 0)
            {
                if (!completedCells.Contains(cells[currentCellIndex - sizeY]) &&
                    !currentPath.Contains(cells[currentCellIndex - sizeY]))
                {
                    possibleDirections.Add(2);
                    possibleNextCells.Add(currentCellIndex - sizeY);

                }
            }

            //Checks if top neighbor is available
            if(currentCellY < sizeY -1)
            {
                if (!completedCells.Contains(cells[currentCellIndex + 1]) &&
                    !currentPath.Contains(cells[currentCellIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextCells.Add(currentCellIndex + 1);
                }
            }

            //Checks if bottom neighbor is available
            if(currentCellY > 0)
            {
                if (!completedCells.Contains(cells[currentCellIndex - 1]) &&
                    !currentPath.Contains(cells[currentCellIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextCells.Add(currentCellIndex - 1);
                }
            }



            //Checks wich walll of the currentcell and neighbor needs te be removed according to the neighbors direction.
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeCell chosenCell = cells[possibleNextCells[chosenDirection]];

                if (chosenCell != null)
                {
                    switch(possibleDirections[chosenDirection])
                    {
                        case 1:
                            chosenCell.RemoveWall(1);
                            currentPath[currentPath.Count -1].RemoveWall(0);
                            break;
                        case 2:
                            chosenCell.RemoveWall(0);
                            currentPath[currentPath.Count -1].RemoveWall(1);
                            break;
                        case 3:
                            chosenCell.RemoveWall(3);
                            currentPath[currentPath.Count -1].RemoveWall(2);
                            break;

                        case 4:
                            chosenCell.RemoveWall(2);
                            currentPath[currentPath.Count -1].RemoveWall(3);
                            break;
                    }
                    
                    currentPath.Add(chosenCell);
                    chosenCell.SetState(CellState.Current);
                }
            }
            else
            {
                //Backtracks by going through the cells in the currentPath list and checking if they have an untouched neighbor.
                if (currentPath[currentPath.Count - 1] != null)
                {
                    completedCells.Add(currentPath[currentPath.Count -1]);

                    currentPath[currentPath.Count - 1].SetState(CellState.Completed);
                    currentPath.RemoveAt(currentPath.Count -1 );
                }
            }
                                    
            if (fastMode == false)
            {
                yield return null;
            }
        }
    }
}
