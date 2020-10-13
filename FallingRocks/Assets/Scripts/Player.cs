using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rock rock;
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
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
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
            Debug.Log(rocks[i].name);
            Destroy(rocks[i]);
        }
    }
    private void OnCollisionEnter2D(Collision2D hitObject)
    {   //Player death upon hitting rock
        if (hitObject.gameObject.tag == "rock")
        {
            //Not sure if I can put the sprite renderer back, so i'm not using this.
            //Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            StartCoroutine(Death());

        }
    }
    IEnumerator Death()
    {
        bool dead = true;
        //fade background & show splash screen
        Debug.Log(deathbg.layer);
        deathbg.DeathBg();
        Debug.Log("You Died");
        text.DeathScreen();
        Debug.Log("Death Screen Plays");
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        Time.timeScale = 0;
        
        
        yield return new WaitForSecondsRealtime(3f);
        rl.DestroyAllRocks();
        deathbg.ResetStatus();
        text.ResetStatus();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
        transform.position = new Vector3(0, -3, 0);
        Time.timeScale = 1;
        StopCoroutine(Death());
        //play sound
        //reset location to start and play respawn animation

    }
}
