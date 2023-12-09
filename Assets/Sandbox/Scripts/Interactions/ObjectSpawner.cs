using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private bool _syncInstance;
    [SerializeField] private GameObject _newGameObject;
    [SerializeField] private Transform _spawnPoint;

    public void OnTriggerEnter(Collider collider)
    {
       
        if(!collider.CompareTag("Player")) return;
        if(_syncInstance)
         {
                Debug.Log("TriggerEnter player");
               SpawnGameObjectServerRPC();}
        else
            {
                
                SpawnLocalGameObjectInstance();
            }
            

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnGameObjectServerRPC()
    {
       NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
       networkObject.Spawn(true);
    }

    //Este código solo se ejecuta del lado del server
    private void SpawnLocalGameObjectInstance()
    {
        if(IsOwner)
        Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity);
    }
}
