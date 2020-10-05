using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLauncher : MonoBehaviour
{
    public GameObject rockPrefab;
    public float respawnTime = 1f;
    private Vector2 screenBounds;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RockSlideWave());

    }
    private void SpawnRock()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        GameObject a = Instantiate(rockPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 2);
    }
    IEnumerator RockSlideWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnRock();

        }
    }
    
}
