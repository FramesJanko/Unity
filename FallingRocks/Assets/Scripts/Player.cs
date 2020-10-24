using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rock rock;

    [SerializeField]
    private SelectObjectOnClick select;
    private float timePassed;
    private float delay;
    [SerializeField]
    private LayerMask platformsLayerMask;
    private Rigidbody2D rb;
    
    //private Transform transform;
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

    GameObject orb1;
    GameObject orb2;
    GameObject orb3;

    [SerializeField]
    private GameObject[] orbPrefabs;

    Vector3 worldPosition;
    // Start is called before the first frame update
    void Start()
    {
        //transform = GetComponent<Transform>();
        isGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        //select = new SelectObjectOnClick();
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

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
        if (Input.GetMouseButtonDown(0) && quasCount == 3)
        {
            StartCoroutine(ColdSnap());
        }




    }
    private void FixedUpdate()
    {
        
    }
    private IEnumerator ColdSnap()
    {
        
        //delay = timePassed + 6f;
        if (select.SelectObject() != null && select.SelectObject().GetComponent<Rigidbody2D>() != null)
        {
            
            GameObject a = select.SelectObject();
            Rigidbody2D selectedRb = a.GetComponent<Rigidbody2D>();
            
            if (!a.GetComponent<Rock>().IsColdsnapped)
            {
                a.GetComponent<Rock>().IsColdsnapped = true;
                Vector2 placeholderVelocity = selectedRb.velocity;
                selectedRb.velocity = new Vector2(0, 0);
                selectedRb.isKinematic = true;
                //while (timePassed < delay)
                //{
                yield return new WaitForSeconds(6f);
                selectedRb.velocity = placeholderVelocity;
                selectedRb.isKinematic = false;
                a.GetComponent<Rock>().IsColdsnapped = false;
            }
            
            //}
        }


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
        
        if (rayCastHit2D.collider == null)
        {
            return false;
        }
        else
        {
            isGrounded = rayCastHit2D.collider.tag == "Platform";
            //Debug.Log(rayCastHit2D.collider.tag + " " + isGrounded);
            return isGrounded;
        }
        

    }
    private void OnCollisionEnter2D(Collision2D hitObject)
    {   //Player death upon hitting rock
        Collider2D myCollider = hitObject.GetContact(0).collider;
        Debug.Log("You struck " + myCollider);
        switch(hitObject.gameObject.tag) //what is hit
        {
            //Not sure if I can put the sprite renderer back, so i'm not using this.
            //Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            case "rock":
                if (myCollider.name == "RockBottomPrefab")
                {
                    if (hitObject.gameObject.GetComponentInParent<Rigidbody2D>().velocity.y != 0)
                    StartCoroutine(Death());
                }
                else if(myCollider.name == "RockTopPrefab")
                {
                    IsGrounded();
                }
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
        StopAllCoroutines();
        //play sound
        //reset location to start and play respawn animation

    }
    public void  CollectOrb(GameObject orb)
    {
        Debug.Log("You collected a " + orb.tag + " orb!");
        if (orb.tag == "quas")
        {
            if (orb1 == null)
            {
                orb1 = Instantiate(orbPrefabs[0], transform.GetChild(2)) as GameObject;

            }
            else if (orb2 == null)
            {
                orb2 = Instantiate(orbPrefabs[1], transform.GetChild(2)) as GameObject;

            }
            else if (orb3 == null)
            {
                orb3 = Instantiate(orbPrefabs[2], transform.GetChild(2)) as GameObject;

            }
            else
            {
                return;
            }
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
