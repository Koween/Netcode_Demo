using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;
using Unity.VisualScripting;

public class GrabableObject : NetworkBehaviour
{

    private Transform _currentParent;
    private bool _followParent;

    private void OnTriggerEnter(Collider collider)
    {
        if(_currentParent == null) return;
        if(collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
             _currentParent = collider.transform;
        }
    }

    void Update()
    {
        if(_followParent)
        
        FollowParentClientRPC();
    }

    [ClientRpc]
    private void FollowParentClientRPC()
    {
        transform.position = _currentParent.transform.position;
    }
}