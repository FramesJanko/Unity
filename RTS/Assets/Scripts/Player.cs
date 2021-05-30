using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    public GameObject player;

    [SyncVar]
    public Vector3 movementLocation;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
    [SyncVar]
    public Vector3 movementLocation;

>>>>>>> Stashed changes
=======
    [SyncVar]
    public Vector3 movementLocation;

>>>>>>> Stashed changes
    Vector3 detourLocation;

    [SyncVar]
    public int totalExperience;

    [SyncVar]
    public Vector3 playerRightClickLocation;

    [SyncVar]
    public bool moveCommandIssued;

    RaycastHit hit;

    [SerializeField]
    private float movespeed;
    
    public GameObject target;

    [SyncVar]
    public bool walking;

    float destinationDistanceFromTarget;

    [SyncVar]
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
    private Vector3 direction;
    private Vector3 startPosition;

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
            //Press Y to start in-game timer;
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartTimer();
            }

            //Look for Right Click locally to determine if server should move the player
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
                direction = worldMousePosition - cam.transform.position;
                startPosition = cam.transform.position;
                SetDirectionandStartPositionOnServer(direction, startPosition);
                RaycastHit rightClickHit;

                if (Physics.Raycast(startPosition, direction, out rightClickHit, 200f, walkableTerrain))
                {
                    Debug.DrawLine(startPosition, rightClickHit.point, Color.green, 0.5f);
                    SetMoveCommand(rightClickHit.point);

                }
                else
                {
                    Debug.DrawLine(startPosition, worldMousePosition, Color.red, 0.5f);
                }
            }

            //Press S to stop movement.
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetStopCommand();
                target = null;
            }
            
            ShowBehindWalls();

            if (walking)
            {
                Debug.DrawLine(transform.position, movementLocation, Color.blue, .1f);

                //CalculateRoute();
            }
            
        }
        if (isServer)
        {
            CheckIfWalking();
            HandleMovement();
        }


<<<<<<< Updated upstream
    }

    [Command]
    private void SetDirectionandStartPositionOnServer(Vector3 ClientDirection, Vector3 ClientCamPosition)
    {
        direction = ClientDirection;
        startPosition = ClientCamPosition;
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
    [Command]
=======
    }

    [Command]
    private void SetDirectionandStartPositionOnServer(Vector3 ClientDirection, Vector3 ClientCamPosition)
    {
        direction = ClientDirection;
        startPosition = ClientCamPosition;
    }

    [Command]
>>>>>>> Stashed changes
    public void SetMoveCommand(Vector3 rightClickHitPoint)
    {
        playerRightClickLocation = rightClickHitPoint;
        moveCommandIssued = true;
    }

    private void LateUpdate()
    {
        transform.position = _navMeshAgent.gameObject.transform.position;
    }
>>>>>>> Stashed changes
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
        if (moveCommandIssued)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
            if(Physics.Raycast(startPosition, direction, out hit, 200f, walkableTerrain))
            {
                CheckForTarget();
>>>>>>> Stashed changes
=======
            if(Physics.Raycast(startPosition, direction, out hit, 200f, walkableTerrain))
            {
                CheckForTarget();
>>>>>>> Stashed changes
=======
            if(Physics.Raycast(startPosition, direction, out hit, 200f, walkableTerrain))
            {
                CheckForTarget();
>>>>>>> Stashed changes
            }
            moveCommandIssued = false;
        }
        

        //destinationDistanceFromTarget = 0f;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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



=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        if (target != null)
        {
            //destinationDistanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
            distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);

            if (/*destinationDistanceFromTarget < 1.5 && */distanceFromTarget < 1.5)
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream


=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        
        if (hit.collider != null && hit.collider.tag == "Loot" && Vector3.Distance(transform.position, hit.transform.position) < 2f)
        {
            movementLocation = transform.position;
        }
>>>>>>> Stashed changes
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

    [Command]
    public void SetStopCommand()
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
    {
        movementLocation = transform.position;
        //Don't need Deselect, as this is already getting called as a Command.
        //Deselect();
        target = null;
    }


    private void CheckForTarget()
>>>>>>> Stashed changes
    {
        movementLocation = transform.position;
        //Don't need Deselect, as this is already getting called as a Command.
        //Deselect();
        target = null;
    }


    private void CheckForTarget()
    {
        if (hit.collider.tag == "Enemy")
        {
            target = hit.collider.gameObject;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            CmdSetTarget();
            Debug.Log($"Targeting {target.name}. Network ID is {target.GetComponent<NetworkIdentity>().netId}");
            
            movementLocation = hit.collider.gameObject.transform.position;
        }
        else
        {
            Deselect();
=======
        }
        else
        {
=======
        }
        else
        {
>>>>>>> Stashed changes
=======
        }
        else
        {
>>>>>>> Stashed changes
            Debug.Log("Deselecting");
            //Deselect();
            target = null;
>>>>>>> Stashed changes
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

