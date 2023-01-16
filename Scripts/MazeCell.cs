using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellState
{
    Available,
    Current,
    Completed,
    Testing
}

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;

    [SerializeField] float wallsWitdh = 0.1f;
    [SerializeField] float wallsLength = 1.1f;


    //Sets wallheight according to user input
    public void SetHeight(int height)
    {
        foreach (GameObject wall in walls)
        {
            wall.transform.localScale = new Vector3(wallsWitdh, height, wallsLength);
        }
    }
    
    public void SetState(CellState state)
    {
        //Definitions for cell states.
        switch(state)
        {
            case CellState.Available:
                floor.material.color = Color.white;
                break;
            case CellState.Current:
                floor.material.color = Color.gray;
                break;
            case CellState.Completed:
                floor.material.color = Color.black;
                break;
        }
    }
    

    public void RemoveWall(int wallToRemove)
    {
        Destroy(walls[wallToRemove].gameObject);
    }




}
