using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        print("Rock = " + transform.position.x);
        print("Screen bound = " + screenBounds.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < (screenBounds.x))
        {
            Destroy(this.gameObject);
        }
    }
}
