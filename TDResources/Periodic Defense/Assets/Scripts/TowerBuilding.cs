using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : MonoBehaviour
{
    public bool prebuilt = true;
    RaycastHit hit;
    Grid grid;
    public LayerMask buildLayer;

    // Update is called once per frame
    private void Awake()
    {
        
        grid = transform.parent.GetComponentInChildren<Grid>();
    }

    void Update()
    {
        if (prebuilt)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - Camera.main.transform.position;

            Physics.Raycast(Camera.main.transform.position, direction, out hit, 200f, buildLayer);
            transform.position = grid.NodeFromWorldPoint(hit.point).worldPosition;
        }
    }

    public RaycastHit PrepareTower()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
        Vector3 direction = worldMousePosition - Camera.main.transform.position;

        Physics.Raycast(Camera.main.transform.position, direction, out hit, 200f, buildLayer);
        return hit;
    }
}
