using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = -30f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, speed, 0);
        if (transform.position.y < (screenBounds.y*-1))
        {
            Destroy(this.gameObject);
        }
        
    }
}
