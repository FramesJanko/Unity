using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLauncher : MonoBehaviour
{
    public GameObject rockPrefab;
    public float respawnTime = 1f;
    public float speed;
    private Vector2 screenBounds;
    private Rigidbody2D rb;
    private Grid grid;

    [SerializeField]
    private Rock rock;

    private Dictionary<int, GameObject> rocks = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    private void Start() {
        
    }
    public void Launcher(float[] launchCoordinates)
    {
        StartCoroutine(RockSlideWave(launchCoordinates));
    }
    private void SpawnRock(float[] launchCoordinates)
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        int random = Random.Range(0, Mathf.FloorToInt(launchCoordinates[2]));
        if (rocks.Count == 4)
        {
            Debug.Log("All rock columns full");

            return;
        }
        while (rocks.ContainsKey(random)){

            

            random =  Random.Range(0, Mathf.FloorToInt(launchCoordinates[2]));
        }
        GameObject a = Instantiate(rockPrefab) as GameObject;
        a.name = "Rock " + random.ToString();
        a.transform.position = new Vector2(launchCoordinates[0] + launchCoordinates[1] * random, 250f);
        Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, speed, 0);
        Debug.Log(a.transform.position.x + " " + a.transform.position.y);
        
        rocks.Add(random, a);
    }
    
    IEnumerator RockSlideWave(float[] launchCoordinates)
    {
        while (true)
        {
            

            yield return new WaitForSeconds(respawnTime);
            

            SpawnRock(launchCoordinates);

        }
    }
    public void DestroyAllRocks()
    {
        GameObject[] rocksArray = GameObject.FindGameObjectsWithTag("rock");
        for (int i = 0; i < rocksArray.Length; i++)
        {
            Debug.Log("Destroying " + rocksArray[i].name);
            
            Destroy(rocksArray[i]);
        }
        rocks.Clear();
    }
    public void PopRocks(int rockID)
    {
        rocks.Remove(rockID);
    }
}
