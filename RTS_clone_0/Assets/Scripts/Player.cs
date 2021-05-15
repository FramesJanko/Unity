using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    
    public Loot desiredLoot;

    [SyncVar]
    public Vector3 movementLocation;
    Vector3 detourLocation;

    [SyncVar]
    public int totalExperience;

    RaycastHit hit;

    [SerializeField]
    private float movespeed;
    
    [SyncVar]
    public GameObject target;

    [SyncVar]
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

    Vector3 tempMovementLocation;
    GameObject tempTarget;
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
        tempMovementLocation = movementLocation;
        walking = false;
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {

        CheckIfWalking();
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartTimer();
            }
            

            HandleMovement();
            tempMovementLocation = movementLocation;
            tempTarget = target;

            
            

            ShowBehindWalls();

            if (walking)
            {
                Debug.DrawLine(transform.position, movementLocation, Color.blue, .1f);

                //CalculateRoute();
            }
            
        }
        target = tempTarget;
        movementLocation = tempMovementLocation;
        if (target != null)
            distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
        
        
    }

    private void SyncTarget(GameObject currentTarget)
    {
        target = currentTarget;
    }

    [Command]
    private void SyncMovement(Vector3 currentMovementLocation)
    {
        movementLocation = currentMovementLocation;
        
    }

    private void LateUpdate()
    {
        transform.position = _navMeshAgent.gameObject.transform.position;
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
            Vector3 startPosition = cam.transform.position;

            if (Physics.Raycast(startPosition, direction, out hit, 200f, walkableTerrain))
            {
                Debug.DrawLine(startPosition, hit.point, Color.green, 0.5f);
                

                CheckForTarget();

                destinationCollider = hit.collider;
                
            }
            else
            {
                Debug.DrawLine(startPosition, worldMousePosition, Color.red, 0.5f);
            }

            destinationDistanceFromTarget = 0f;

            if (target != null)
            {
                
                destinationDistanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            movementLocation = transform.position;
            Deselect();
            target = null;
            
        }
        if (target != null)
        {
            
            distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);


            if (destinationDistanceFromTarget < 1.5 && distanceFromTarget < 1.5)
            {
                movementLocation = transform.position;
            }
        }
        if (hit.collider != null && hit.collider.tag == "Loot" && Vector3.Distance(transform.position, hit.transform.position) < 2f)
        {
            movementLocation = transform.position;
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
        Debug.Log(hit.collider.tag);

        if (hit.collider.tag == "Enemy")
        {
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - cam.transform.position;
            Vector3 startPosition = cam.transform.position;

            SetTarget(startPosition, direction);
            //Debug.Log($"Targeting {target.name}. Network ID is {target.GetComponent<NetworkIdentity>().netId}");
            
            //movementLocation = target.transform.position;
        }
        else
        {
            Debug.Log("Deselecting");
            Deselect();
            target = null;
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
        Debug.Log("Running Deselect Command");
        target = null;
    }
    [Command]
    public void StartTimer()
    {
        textScript.needToSetFirstTime = true;
        textScript.readyToShowTime = true;
    }
    [Command]
    public void SetTarget(Vector3 startPosition, Vector3 direction)
    {
        RaycastHit targetHit;

        if (Physics.Raycast(startPosition, direction, out targetHit, 200f, walkableTerrain))
        {
            Debug.Log(targetHit.collider.name);
            target = targetHit.collider.gameObject;
        }
            
        //GetComponent<Combat>().target = target;
    }
    
}

