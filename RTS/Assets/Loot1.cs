using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Loot : NetworkBehaviour
{
    [SyncVar]
    public bool isOpen;
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            _animator.SetBool("isOpen", true);
        }
    }
}
