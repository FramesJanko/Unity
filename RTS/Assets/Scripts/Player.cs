using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    public GameObject player;

    Vector3 movementLocation;
    Vector3 detourLocation;

    [SyncVar]
    public int totalExperience;

    RaycastHit hit;

    [SerializeField]
    private float movespeed;
    
    public GameObject target;

    public bool walking;

    float destinationDistanceFromTarget;

    public float distanceFromTarget;

    public Collider destinationCollider;

    //This is for determining where the player can move to
    [SerializeField]
    LayerMask walkableTerrain;

    //This is for turning objects in front of the player transparent or opaque
    MeshRenderer lastHitMeshRenderer;

    NavMeshAgent _navMeshAgent;

    [SerializeField]
    TextScript textScript;

    Camera cam;

    Animator animator;

    private Combat _combat;
    private bool hasAnimator;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        textScript = GameObject.Find("Time").GetComponent<TextScript>();
        _combat = GetComponent<Combat>();
        totalExperience = 0;
        if (GetComponent<Animator>())
        {
            hasAnimator = true;
            animator = GetComponent<Animator>();

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        movementLocation = transform.position;
        walking = false;
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartTimer();
            }
            CheckIfWalking();

            HandleMovement();

            

            ShowBehindWalls();

            if (walking)
            {
                Debug.DrawLine(transform.position, movementLocation, Color.blue, .1f);

                //CalculateRoute();
            }
            
        }
        
        
    }

    private void ShowBehindWalls()
    {
        
        
        Ray ray = new Ray(cam.transform.position, transform.position - cam.transform.position);
        Debug.DrawRay(cam.transform.position, (transform.position - cam.transform.position), Color.cyan, .1f);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Vector3.Distance(cam.transform.position, transform.position)))
        {
            

            if (hit.collider.gameObject.layer == 8)
            {
                lastHitMeshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                hit.collider.gameObject.GetComponent<ShowPlayer>().SetTransparentMaterial();

            }
            else
            {
                if (lastHitMeshRenderer != null)
                {
                    lastHitMeshRenderer.gameObject.GetComponent<ShowPlayer>().SetOpaqueMaterial();
                }
            }


        }
        else
        {
            if (lastHitMeshRenderer != null)
            {
                lastHitMeshRenderer.gameObject.GetComponent<ShowPlayer>().SetOpaqueMaterial();
            }
        }
        
            
    }
    private void CalculateRoute()
    {
        if (Physics.Raycast(transform.position, movementLocation, out hit, 200f))
        {
            Debug.Log(hit.collider);

            Debug.Log(destinationCollider);
            Debug.DrawLine(transform.position, movementLocation, Color.yellow, 1f);

            //if (hit.collider != destinationCollider)
            //{
            //    Debug.DrawLine(transform.position, movementLocation, Color.yellow, 1f);

            //}

        }
    }

    private void HandleMovement()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - cam.transform.position;

            if (Physics.Raycast(cam.transform.position, direction, out hit, 200f, walkableTerrain))
            {
                Debug.DrawLine(cam.transform.position, hit.point, Color.green, 0.5f);
                

                CheckForTarget();

                destinationCollider = hit.collider;
                
            }
            else
            {
                Debug.DrawLine(cam.transform.position, worldMousePosition, Color.red, 0.5f);
            }

            destinationDistanceFromTarget = 0f;

            if (target != null)
            {
                float destinationDistanceFromTargetX = Math.Abs(movementLocation.x - target.transform.position.x);
                float destinationDistanceFromTargetZ = Math.Abs(movementLocation.z - target.transform.position.z);
                destinationDistanceFromTarget = destinationDistanceFromTargetX * destinationDistanceFromTargetX;
                destinationDistanceFromTarget += (destinationDistanceFromTargetZ * destinationDistanceFromTargetZ);
            }




            
            //Debug.Log("Distance from target: " + System.Math.Sqrt(distanceFromTarget));



        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            movementLocation = transform.position;
            target = null;
            
        }



        if (target != null)
        {
            float distanceFromTargetX = System.Math.Abs(transform.position.x - target.transform.position.x);
            float distanceFromTargetZ = System.Math.Abs(transform.position.z - target.transform.position.z);
            distanceFromTarget = distanceFromTargetX * distanceFromTargetX;
            distanceFromTarget += (distanceFromTargetZ * distanceFromTargetZ);


            if (destinationDistanceFromTarget < 1.5 && distanceFromTarget < 1.5)
            {
                //if (System.Math.Abs(movementLocation.x - target.transform.position.x) > 1.5 && System.Math.Abs(movementLocation.z - target.transform.position.z) > 1.5)
                //{
                //    movementLocation = hit.point + new Vector3(0f, player.GetComponent<MeshRenderer>().bounds.size.y / 2f, 0f);
                //}
                //else
                //{

                //    movementLocation = transform.position;
                //}
                movementLocation = transform.position;
            }
        }


        if (target != null && distanceFromTarget > _combat.baseAttackRange && !_combat.isAttacking)
        {
            movementLocation = target.transform.position;
        }
        if (target != null && distanceFromTarget <= _combat.baseAttackRange*.9)
        {
            movementLocation = transform.position;
        }
        _navMeshAgent.SetDestination(movementLocation);
        //transform.position = Vector3.MoveTowards(transform.position, movementLocation, movespeed * Time.deltaTime);
    }

    
    
    private void CheckForTarget()
    {
        if (hit.collider.tag == "Enemy")
        {
            target = hit.collider.gameObject;
            CmdSetTarget();
            Debug.Log($"Targeting {target.name}. Network ID is {target.GetComponent<NetworkIdentity>().netId}");
            
            movementLocation = hit.collider.gameObject.transform.position;
        }
        else
        {
            Deselect();
            movementLocation = hit.point;

        }
    }

    public void CheckIfWalking()
    {

        walking = (movementLocation.x != transform.position.x && movementLocation.y != transform.position.z);
        
    }
    [Command]
    public void Deselect()
    {
        target = null;
    }
    [Command]
    public void StartTimer()
    {
        textScript.needToSetFirstTime = true;
        textScript.readyToShowTime = true;
    }
    
    public void CmdSetTarget()
    {
        GetComponent<Combat>().target = target;
    }
}

