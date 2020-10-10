using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLauncher : MonoBehaviour
{
    public GameObject rockPrefab;
    public float respawnTime = 1f;
    public float speed = -30;
    private Vector2 screenBounds;
    private Rigidbody2D rb;
    private Grid grid;
    // Start is called before the first frame update
    private void Start() {
        respawnTime = 1f;
        
    }
    public void Launcher(float[] launchCoordinates)
    {
        StartCoroutine(RockSlideWave(launchCoordinates));
    }
    private void SpawnRock(float[] launchCoordinates)
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        GameObject a = Instantiate(rockPrefab) as GameObject;
        a.transform.position = new Vector2(launchCoordinates[0] + launchCoordinates[1] * Random.Range(0, Mathf.FloorToInt(launchCoordinates[2])), screenBounds.y * 1.1f);
        Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, speed, 0);
        Debug.Log(launchCoordinates[0] + " " + launchCoordinates[1] + " " + launchCoordinates[2]);
    }
    IEnumerator RockSlideWave(float[] launchCoordinates)
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnRock(launchCoordinates);

        }
    }
    
}
