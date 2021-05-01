using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LootCollector : NetworkBehaviour
{
    Camera cam;

    public RaycastHit hit;


    [SerializeField]
    private LayerMask loot;
    private GameObject tempDesiredLoot;
    [SyncVar]
    public GameObject desiredLoot;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
                Vector3 direction = worldMousePosition - cam.transform.position;
                Vector3 startPosition = cam.transform.position;
                SetDesiredLoot(direction, startPosition);

            }
            if (desiredLoot != null && Vector3.Distance(gameObject.transform.position, desiredLoot.transform.position) < 5f && desiredLoot.GetComponent<Loot>().isOpen == false)
            {
                
                OpenLoot();

                
            }
        }
    }

    [Command]
    public void SetDesiredLoot(Vector3 direction, Vector3 startPosition)
    {
        if (Physics.Raycast(startPosition, direction, out hit, 200f, loot))
        {
            Debug.Log("Hit " + hit.collider.name);
            desiredLoot = hit.collider.gameObject;
            
        }
        else
        {
            desiredLoot = null;
        }
        
    }
    [Command]
    public void OpenLoot()
    {
        desiredLoot.GetComponent<Loot>().isOpen = true;
    }
}
