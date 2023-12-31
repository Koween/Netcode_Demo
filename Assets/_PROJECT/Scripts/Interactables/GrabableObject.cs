using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;
using Unity.VisualScripting;

public class GrabableObject : OnTriggerInteractableObj
{
    [SerializeField] private Transform _currentParent;
    private float _zOffsetPosition = .5f;

    private void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if(_currentParent != collider.gameObject)
            SetCurrentParentServerRpc(collider.transform.GetComponent<NetworkObject>());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetCurrentParentServerRpc(NetworkObjectReference player)
    {
        SetCurrentParentClientRpc(player);
    }

    [ClientRpc]
    private void SetCurrentParentClientRpc(NetworkObjectReference player)
    {
        player.TryGet(out NetworkObject obj);
        _currentParent = obj.transform;
    }


    //The grabable object must have a clientnetwork transform in order to work 
    void Update()
    {
        if(_currentParent)
        FollowParent();
        
    }

    
    private void FollowParent()
    {
        Vector3 objPosition = _currentParent.transform.position;
        objPosition += _currentParent.transform.forward * _zOffsetPosition;
        transform.position = objPosition;
    }
    
/*
    Server authoritive works for this case but his method requires a component networkTransform to work attached 
    to the grabable object
    void Update()
    {
        if(_followParent)
        FollowParentServerRpc(_currentParent.GetComponent<NetworkObject>());
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void FollowParentServerRpc(NetworkObjectReference player)
    {
        player.TryGet(out NetworkObject obj);
        Debug.Log(obj.transform.position);
        transform.position = obj.transform.position;
    }
    */
}