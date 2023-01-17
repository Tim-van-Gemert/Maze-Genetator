using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell cellPrefab;
    [SerializeField] GameObject mazeRunner;
    [SerializeField] GameObject cellFX;
    [SerializeField] TMP_InputField mazeWitdh;
    [SerializeField] TMP_InputField mazeHeight;
    [SerializeField] TMP_InputField mazeWallHeight;
    [SerializeField] TMP_Text heightWarning;
    [SerializeField] TMP_Text witdhWarning;
    [SerializeField] GameObject prettyButton;
    [SerializeField] GameObject particleButton;
    [SerializeField] GameObject lightButton;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject directionalLight;


    public int mazeSizeX;
    public int mazeSizeY;
    public int sizeX;
    public int sizeY;

    public int wallHeight;
    private bool widthIsNumber = false;
    private bool heightIsNumber = false;
    private bool wallHeightIsNumber = false;
    private Coroutine lastMaze = null;

    public bool fastMode = false;
    private bool particleMode = true;
    private bool lightMode = true;

    private GameObject[] cellsOnScreen;
    private GameObject[] FXOnScreen;

    public List<MazeCell> cells = new List<MazeCell>();  
    public List<MazeCell> currentPath = new List<MazeCell>();
    public List<MazeCell> completedCells = new List<MazeCell>(); 

    void Update() 
    {

        widthIsNumber = int.TryParse(mazeWitdh.text, out mazeSizeX);
        heightIsNumber = int.TryParse(mazeHeight.text, out mazeSizeY);
        wallHeightIsNumber = int.TryParse(mazeWallHeight.text, out wallHeight);

        //Checks height and witdh and displays warning if needed.

        
        if (mazeSizeX > 250)
        {
            witdhWarning.text = "Can't be higher than 250!";
        }
        else if (mazeSizeX >= 150) 
        {
            witdhWarning.text = "May take some time!";
        } 
        else if (mazeSizeX < 10 && mazeSizeX > 0 )
        {
            witdhWarning.text = "Must be at least 10";
        } 
        else
        {
           witdhWarning.text = "";
        }

        if (mazeSizeY > 250)
        {
            heightWarning.text = "Can't be higher than 250!";
        } 
        else if (mazeSizeY >= 150) 
        {
            heightWarning.text = "May take some time!";
        }
        else if (mazeSizeY < 10 && mazeSizeY > 0)
        {
            heightWarning.text = "Must be at least 10";
        } 
        else 
        {
           heightWarning.text = "";
        }
    }

    public void PrepareGeneration()
    {   
            if (widthIsNumber == true &&
                heightIsNumber == true &&
                wallHeightIsNumber == true )  
            {

                if (mazeSizeX <= 250 && mazeSizeX >= 10 && mazeSizeY <= 250 && mazeSizeY >= 10 )
                {
                    //Removes meme from screen
                    Destroy(GameObject.Find("No Mazes"));

                    //Sets the height of the camera to see the maze in its entirety
                    Vector3 camPos = new Vector3(0, mazeSizeX + mazeSizeY, 0);
                    cam.transform.position = camPos;
                    cam.GetComponent<CameraController>().SetPos(camPos);

                    //Destroy maze if it is already created.
                    if (lastMaze != null)
                    {
                        StopCoroutine(lastMaze); 
                    }

                    SetWallHeight(wallHeight);

                    lastMaze = StartCoroutine(GenerateMaze(mazeSizeX, mazeSizeY)); 

                }
            }
    }

    //Set wall height
    public void SetWallHeight(int wallHeight)
    {
        cellPrefab.GetComponent<MazeCell>().SetHeight(wallHeight);
    }



    IEnumerator GenerateMaze(int sizeX, int sizeY)
    {
        cellsOnScreen = GameObject.FindGameObjectsWithTag("Cell");
        FXOnScreen = GameObject.FindGameObjectsWithTag("CellFX");

        //Clears screen by removing all maze objects
        foreach (GameObject item in cellsOnScreen)
        {
            Destroy(item);
        }

        foreach (GameObject item in FXOnScreen)
        {
            Destroy(item);
        }

        cells = new List<MazeCell>();  
        currentPath = new List<MazeCell>();
        completedCells = new List<MazeCell>(); 

        //Assigns randoms cells to become the entrance and exit.
        int randomEntrance = Random.Range(0, sizeX);
        int randomExit = Random.Range(0, sizeX);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector3 cellPos = new Vector3(x - (sizeX / 2f), 0, y - (sizeY / 2.15f));

                MazeCell newCell = Instantiate(cellPrefab, cellPos, Quaternion.identity, transform);

                if (particleMode)
                {
                    if (y == 0 | x == 0 | x == sizeX - 1 | y == sizeY - 1)
                    {
                        Instantiate(cellFX, new Vector3(cellPos.x, -3, cellPos.z), Quaternion.identity, transform);        
                    }
                }

                //Create the entrance of the maze
                if (x == randomEntrance && y == 0)
                {
                    newCell.RemoveWall(3);
                }

                //Create the exit of the maze
                if (x == randomExit && y == sizeY -1)
                {
                    newCell.RemoveWall(2);
                }

                cells.Add(newCell);

                if (!fastMode)
                {
                    yield return null;
                }
            }
        }

        mazeRunner.GetComponent<MazeRunner>().EnableRunners();
    }



    //Toggles between pretty and non pretty generation mode. The StartGeneration IEnumerator has 2 yield return null when pretty mode is active. 
    public void ToggleFastMode()
    {  
        fastMode = !fastMode;

        if (fastMode == false)
        {
            prettyButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
           prettyButton.GetComponent<Image>().color = Color.green;
        }
    }

    //Toggles particles on or off depending on the status.
    public void ToggleParticles()
    {  
        particleMode = !particleMode;

        if (particleMode == false)
        {
            particleButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
           particleButton.GetComponent<Image>().color = Color.green;
        }
    }

    //Toggles directional light on or off depending on the status.
    public void ToggleLights()
    {  
        lightMode = !lightMode;

        if (lightMode == false)
        {
            lightButton.GetComponent<Image>().color = Color.red;
            directionalLight.GetComponent<Light>().enabled = false;
        }
        else
        {
            lightButton.GetComponent<Image>().color = Color.green;
            directionalLight.GetComponent<Light>().enabled = true;
        }
    }

}
