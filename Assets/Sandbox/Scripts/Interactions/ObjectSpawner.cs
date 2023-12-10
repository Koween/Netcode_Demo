using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private bool _syncInstance;
    [SerializeField] private GameObject _newGameObject, _currentPlayer;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TextMesh _debugger;

    public void OnTriggerEnter(Collider collider)
    {
       
        if(!collider.CompareTag("Player")) return;
        _currentPlayer = collider.gameObject;
        if(_syncInstance)
         {
            //if you delete IsOwner conditional will appear a GameObject Instance for every client conected
            //due that code will execute in all clients.
            //In this case due that the Owner of this spawner is the server it will just be executed in the server side.
            //If you want to let players to execute this besides to take out the conditionl you must use the param owner required
            //as false
            
            if(IsOwner)
               SpawnOneGameObjectForAllClientsServerRpc();
        }
        else
            {
                ulong clientId = collider.GetComponent<NetworkObject>().OwnerClientId;
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientId }
                    }
                };
                //SpawnLocalGameObjectInstanceClientRPC(clientRpcParams);
                SpawnLocalGameObjectInstance();
            }
            

    }

    [ServerRpc]
    public void SpawnOneGameObjectForAllClientsServerRpc()
    {
        Debug.Log("SPAWN");
        Debug.Log(gameObject.GetComponent<NetworkObject>().OwnerClientId);
        NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
        networkObject.Spawn(true);
        //networkObject.TrySetParent(_currentPlayer);
       // Vector3 objPosition = _currentPlayer.transform.position;
       // objPosition.z += 0.5f;
       // networkObject.transform.position = objPosition;
    }

    /*[ServerRpc]
    public void SpawnOneGameObjectForAllClientsServerRpc()
    {
        NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
        networkObject.Spawn(true);
        networkObject.TrySetParent(_currentPlayer);
        //Vector3 objPosition = _currentPlayer.transform.position;
        //objPosition.z += 0.5f; 
        //networkObject.transform.position = objPosition;
        SetSpawnedObjectPositionClientRPC(networkObject);
    }

    [ClientRpc]
    private void SetSpawnedObjectPositionClientRPC(NetworkObjectReference objectReference)
    {
        objectReference.TryGet(out NetworkObject obj);
        Vector3 objPosition = _currentPlayer.transform.position;
        objPosition.z += 0.5f; 
        obj.transform.position = objPosition;
    }
    */

    //Just in the specified clients in the clientParams
    /*[ClientRpc]
    private void SpawnLocalGameObjectInstanceClientRPC( ClientRpcParams clientRpcParams = default)
    {
      GameObject Instance =  Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity);
      //Instance.transform.SetParent(_currentPlayer.transform);
    }*/

    private void SpawnLocalGameObjectInstance()
    {
      GameObject Instance =  Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity);
    }

    
}
