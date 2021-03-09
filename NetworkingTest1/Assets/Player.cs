using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    public float speed = .1f;

    [SyncVar]
    public Color32 _Color;
    [SyncVar]
    public float buffTimer = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        _Color = GetComponent<MeshRenderer>().material.color;
        Debug.Log($"Color: {_Color}");
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleMovement();
            
            
        }

        GetComponent<MeshRenderer>().material.color = _Color;
        
    }
    public override void OnStartClient()
    {
        Debug.Log(GetComponent<NetworkIdentity>().netId);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"You hit {collision.gameObject.name}");
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Trigger: You hit {collision.gameObject.name}");
        if (collision.gameObject.name == "GrowBall")
        {
            Buff();

        }
    }
    private void HandleMovement()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position = new Vector3(transform.position.x + (Input.GetAxis("Vertical") * speed), transform.position.y, transform.position.z);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (Input.GetAxis("Horizontal") * speed));
        }
    }
    
    [Command]
    private void Buff()
    {
        
        transform.localScale *= 2;
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject.Find("GrowBall").SetActive(false);
        StartCoroutine(TurnOnBuff());
    }
    private IEnumerator TurnOnBuff()
    {
        yield return new WaitForSeconds(buffTimer);
        GameObject.Find("GrowBall").SetActive(true);

    }
}
