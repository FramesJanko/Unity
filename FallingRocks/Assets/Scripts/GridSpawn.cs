using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridSpawn : MonoBehaviour
{
    private Grid grid;
    private int count;
    public RockLauncher rl;
    //private RockLauncher rl;
    

    
    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid(4, 10, 17.5f, new Vector3(-35, -5));
        
        
        count = 0;
        rl.Launcher(grid.LaunchCoordinates());
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), count);
            count++;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
            
        }
    }

    
}
