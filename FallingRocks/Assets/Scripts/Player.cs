using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    

    private Rigidbody2D rb;

    public float jumpForce = 7;

    public BoxCollider2D col;

    public float moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-1*moveSpeed, rb.velocity.y);
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        


        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
