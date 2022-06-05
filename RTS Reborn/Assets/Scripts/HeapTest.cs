using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeapTest : MonoBehaviour
{

    public GameObject prefab;
    Heap<Brick> bricks;
    float brickSpacing = 1.5f;
    int size = 36;
    public GameObject startPosition;
    Vector3 startPositionTransform;
    Vector3[] heapVisual;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        bricks = new Heap<Brick>(size);
        startPositionTransform = startPosition.transform.position;
        heapVisual = new Vector3[size];
        heapVisual[0] = startPositionTransform;
        foreach (Vector3 vector in heapVisual)
        {

        }
        Brick[] bricksArray = bricks.items;
        //This creates an array of bricks and assigns it to the Heap of bricks.
        for (int i = 0; i < size; i++)
        {
            
            if (i == 0)
                bricksArray[i] = new Brick(size, Random.Range(0, 50), Random.Range(0, 50), i);
            else
                bricksArray[i] = new Brick(size, Random.Range(0, 50), Random.Range(0, 50), i, bricksArray[(i - 1) / 2]);
        }
        bricks.items = bricksArray;

        

    }

    public void ShowArray(int size, Vector3 position, GameObject prefab, Heap<Brick> heap)
    {
        Brick[] brickPile = heap.items;
        for (int i = 0; i < size; i++)
        {

            
            if (i == 0)
            {

                prefab.transform.position = position;
                brickPile[i].Position = position;
            }

            else
            {
                Debug.Log($"brick {i} has been created. it's the {brickPile[i].Left} child of {brickPile[i].parent.HeapIndex}");
                brickPile[i].CountRow(brickPile[i]);
                prefab.transform.position = brickPile[i].Position;
                prefab.GetComponentInChildren<TextMeshProUGUI>().text = brickPile[i].fCost.ToString();
                Debug.Log(brickPile[i].rowSpacing[brickPile[i].rowSpacingIndex]);

            }
            

            Instantiate(prefab);


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowArray(size, startPositionTransform, prefab, bricks);

        }
    }
}

public class Brick : IHeapItem<Brick>
{
    private int heapIndex;
    public int fCost;
    int gCost;
    int hCost;
    public Brick parent;
    public bool left;
    public float spacingModifier = .25f;
    Vector3 position;
    public int rowCount = 0;
    int brickCount;
    public int[] rowSpacing = new int[] { 0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
    public int rowSpacingIndex;

    public Vector3 Position
    {
        get
        {
            if(parent == null)
            {
                return position;
            }
            else
            {
                CheckRowSpacing(HeapIndex);
                if (Left)
                    return parent.Position + new Vector3(-spacingModifier * rowSpacing[rowSpacingIndex], 0, -spacingModifier * 2);
                else
                    return parent.Position + new Vector3(spacingModifier* rowSpacing[rowSpacingIndex], 0, -spacingModifier * 2);
            }
        }
        set
        {
             position = value;
        }
    }

    private void CheckRowSpacing(int heapIndex)
    {
        int tempBrickCount = brickCount;
        rowSpacingIndex = 0;
        while (tempBrickCount >= 2)
        {
            tempBrickCount = tempBrickCount / 2;
            rowSpacingIndex++;
        }
        rowSpacingIndex -= rowCount;
    }

    public bool Left
    {
        get
        {
            return parent.HeapIndex * 2 + 1 == HeapIndex;
        }
    }

    public int HeapIndex 
    { 
        get { return heapIndex;} 
        set { heapIndex = value; } 
    }

    public Brick(int _brickCount, int _fcost, int _hcost, int _heapIndex, Brick _parent)
    {
        brickCount = _brickCount;
        fCost = _fcost;
        hCost = _hcost;
        HeapIndex = _heapIndex;
        parent = _parent;
    }
    public Brick(int _brickCount, int _fcost, int _hcost, int _heapIndex)
    {
        brickCount = _brickCount;
        fCost = _fcost;
        hCost = _hcost;
        HeapIndex = _heapIndex;
    }
    public int CompareTo(Brick other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }

    public void CountRow(Brick brick)
    {
        rowCount = -1;
        Brick currentBrick = brick;
        while(currentBrick.parent != null)
        {
            rowCount++;
            currentBrick = currentBrick.parent;
        }
    }
}
