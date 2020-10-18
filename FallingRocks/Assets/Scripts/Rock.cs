using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float speed = -30f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;

    [SerializeField]
    private RockLauncher rl;

    [SerializeField]
    private GameObject rock;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        rl = (RockLauncher)FindObjectOfType(typeof(RockLauncher));

    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector3(0, speed, 0);
        if (transform.position.y < -30)
        {
            rl.PopRocks(int.Parse(name.Substring(5, 1)));
            Destroy(gameObject);
        }
        
    }
    private void FixedUpdate()
    {
        //var currentVelocity = rb.velocity;

        //if (currentVelocity.y <= 0f)
        //    return;

        //currentVelocity.y = 0f;

        //rb.velocity = currentVelocity;
    }
}
