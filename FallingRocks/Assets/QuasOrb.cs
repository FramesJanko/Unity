using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuasOrb : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private void Start()
    {
        
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.CollectOrb(gameObject);
            //show up around player
            Destroy(gameObject);
        }
            
        else
        {
            return;
        }
    }
}
