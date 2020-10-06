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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RockSlideWave());

    }
    private void SpawnRock()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        GameObject a = Instantiate(rockPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-35, 35), screenBounds.y *1.1f);
        Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, speed, 0);
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
