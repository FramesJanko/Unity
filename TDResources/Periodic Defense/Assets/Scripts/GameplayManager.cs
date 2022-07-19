using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public GameObject basicElemental;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    int waveSize;
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    Transform targetPos;
    public List<GameObject> spawnedUnits;
    public GameObject[] towersAvailable;
    public bool preparingTowerPlacement;
    RaycastHit hit;
    public GameObject preparedTower;
    Grid grid;
    public GameObject selectionIndicatorPrefab;
    public GameObject selectionIndicator;
    public GameObject selectedObject;
    bool unitSelected;
    public LayerMask unitLayer;
    public GameObject buildCanvas;
    
    //public LayerMask buildLayer;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInChildren<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //March _march = new March(gameObject);
            //March[] _array = new March[] { _march };
            SphereCreator _sc = gameObject.AddComponent<SphereCreator>();
            _sc.Initialize(spawnInterval, waveSize, /*_array, */basicElemental, spawnPoint.position, targetPos);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!preparingTowerPlacement)
            {
                
                buildCanvas.SetActive(true);
                
                

            }

            else
            {
                
                buildCanvas.SetActive(false);
            }
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (preparingTowerPlacement)
            {
                
                preparingTowerPlacement = false;
                preparedTower.GetComponent<TowerAppearance>().Build();
                preparedTower.GetComponent<TowerBuilding>().prebuilt = false;
                grid.UpdateWalkable(preparedTower.transform.position, preparedTower.GetComponent<BasicTower>().towerSize*grid.nodeRadius*.95f, false);
                for (int i = 0; i < spawnedUnits.Count; i++)
                {

                    spawnedUnits[i].GetComponent<Unit>().BeginPath();
                }
            }
            else 
            {
                
                

                ClickSelect();
            }
            
        }
        
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    towerSize += 2;
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    towerSize -= 2;
        //}
    }

    private void ClickSelect()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
        Vector3 direction = worldMousePosition - Camera.main.transform.position;
        
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 200f, unitLayer))
        {
            selectedObject = hit.collider.gameObject;
            Debug.Log(selectedObject.name);
            if (!unitSelected)
            {
                selectionIndicator = Instantiate(selectionIndicatorPrefab, selectedObject.transform);
                unitSelected = true;
            }
            else
            {
                
                selectionIndicator.transform.parent = selectedObject.transform;
                selectionIndicator.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            unitSelected = false;
            Destroy(selectionIndicator);
        }

    }
    public void TowerButton(int i)
    {
        preparingTowerPlacement = true;
        preparedTower = towersAvailable[i];
        hit = preparedTower.GetComponent<TowerBuilding>().PrepareTower();
        preparedTower = Instantiate(preparedTower, hit.point, Quaternion.identity, transform);
        preparedTower.GetComponent<TowerAppearance>().Prebuild();
        preparedTower.GetComponent<TowerBuilding>().prebuilt = true;
        buildCanvas.SetActive(false);
    }
}
