using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Camera cam;
    public Vector3 blinkTarget;
    public bool blinkTargetSet;
    public RaycastHit hitInfo;
    public bool blinkPrepared;
    public bool quickCast;
    public bool cooldownReady;
    public float currentCooldown;
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        currentCooldown = 0f;
        cooldownReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cooldownReady)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                cooldownReady = true;
                currentCooldown = cooldown;
            }
        }
        
        if (quickCast)
        {
            if (Input.GetKeyDown(KeyCode.B)/* && cooldownReady*/)
            {
                Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
                Vector3 direction = worldMousePosition - cam.transform.position;
                Vector3 startPosition = cam.transform.position;

                Physics.Raycast(startPosition, direction, out hitInfo, 200f);

                blinkTarget = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                transform.position = blinkTarget;
                blinkPrepared = false;
                cooldownReady = false;
                currentCooldown = cooldown;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                PrepareBlink();
            }
            if (Input.GetMouseButtonDown(0) && blinkPrepared)
            {
                Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
                Vector3 direction = worldMousePosition - cam.transform.position;
                Vector3 startPosition = cam.transform.position;

                Physics.Raycast(startPosition, direction, out hitInfo, 200f);

                blinkTarget = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                blinkTargetSet = true;
            }
            if (blinkTargetSet/* && cooldownReady*/)
            {
                CastBlink();
            }

        }

    }

    public void CastBlink()
    {
        transform.position = blinkTarget;
        blinkPrepared = false;
        cooldownReady = false;
        currentCooldown = cooldown;
        blinkTargetSet = false;
    }

    public void PrepareBlink()
    {
        blinkPrepared = true;

        if (cooldownReady || currentCooldown <= .9f)
        {

        }    
    }
}
