using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rock rock;

    [SerializeField]
    private LayerMask platformsLayerMask;
    private Rigidbody2D rb;
    
    private Transform transform;
    public float jumpForce = 7;

    public BoxCollider2D col;

    public float moveSpeed = 3f;

    [SerializeField]
    private RockLauncher rl;

    private bool isGrounded;
    
    [SerializeField]
    private textLayer text;

    [SerializeField]
    private DeathBackground deathbg;

    private int quasCount = 0;
    private int wexCount = 0;
    private int exortCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        isGrounded = true;
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
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            
            rb.velocity = Vector2.up * jumpForce;
            isGrounded = false;
        }
        
    }
    private void FixedUpdate()
    {
        
    }
    public void DestroyAllRocks()
    {
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("rock");
        for (int i = 0; i < rocks.Length; i++)
        {
            //Debug.Log(rocks[i].name);
            Destroy(rocks[i]);
        }
    }
    private bool IsGrounded()
    {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(/*origin*/col.bounds.center, /*size*/col.bounds.size * .93f, /*angle*/0f, /*direction*/Vector2.down, .18f, platformsLayerMask.value);
        
        isGrounded = rayCastHit2D.collider.tag == "Platform";
        //Debug.Log(rayCastHit2D.collider.tag + " " + isGrounded);
        return isGrounded;

    }
    private void OnCollisionEnter2D(Collision2D hitObject)
    {   //Player death upon hitting rock
        switch(hitObject.gameObject.tag) //what is hit
        {
            //Not sure if I can put the sprite renderer back, so i'm not using this.
            //Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            case "rock":
                StartCoroutine(Death());
                break;
            case "Platform":
                IsGrounded();
                break;
        }
    }
    private void OnCollisionStay2D(Collision2D hitObject)
    {
        //switch (hitObject.gameObject.tag)// what is being hit
        //{
        //    case "Platform":
        //        IsGrounded();
        //        break;
        //}

    }
    private void OnCollisionExit2D(Collision2D hitObject)
    {
        switch(hitObject.gameObject.tag)
        {
            case "Platform":
                isGrounded = false;
                break;
        }
    }
    IEnumerator Death()
    {
        bool dead = true;
        //fade background & show splash screen
        //Debug.Log(deathbg.layer);
        deathbg.DeathBg();
        //Debug.Log("You Died");
        text.DeathScreen();
        //Debug.Log("Death Screen Plays");
        rb.constraints =
            RigidbodyConstraints2D.FreezePosition |
            RigidbodyConstraints2D.FreezeRotation;

        Time.timeScale = 0;
        
        
        yield return new WaitForSecondsRealtime(3f);
        rl.DestroyAllRocks();
        deathbg.ResetStatus();
        text.ResetStatus();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
        transform.position = new Vector3(0, -3, 0);
        Time.timeScale = 1;
        
        //play sound
        //reset location to start and play respawn animation

    }
    public void  CollectOrb(GameObject orb)
    {
        Debug.Log("You collected a " + orb.tag + " orb!");
        if (orb.tag == "quas")
        {
            quasCount++;
        }
        else if (orb.tag == "wex")
        {
            wexCount++;
        }
        else if (orb.tag == "exort")
        {
            exortCount++;
        }
        Debug.Log(quasCount + " " + wexCount + " " + exortCount);
    }
}
