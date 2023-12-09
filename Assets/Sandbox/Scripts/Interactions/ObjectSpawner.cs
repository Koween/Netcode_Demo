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

    public void OnTriggerEnter(Collider collider)
    {
       
        if(!collider.CompareTag("Player")) return;
        if(_syncInstance)
         {
            if(IsOwnedByServer)
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
                _currentPlayer = collider.gameObject;
                SpawnLocalGameObjectInstanceClientRPC(clientRpcParams);
            }
            

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnOneGameObjectForEveryClientServerRpc()
    {
       NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
       networkObject.Spawn(true);
    }

    /*//[ServerRpc(RequireOwnership = false)]
    public void SpawnOneGameObjectForAllClients(Transform player)
    {
        NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
        //Debug.Log(networkObject.TrySetParent(player));
        networkObject.setp
    }*/

    //[ServerRpc(RequireOwnership = false)]
    [ServerRpc]
    public void SpawnOneGameObjectForAllClientsServerRpc()
    {
        NetworkObject networkObject = Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
        networkObject.Spawn(true);
    }

    //Just in the specified clients in the clientParams
    [ClientRpc]
    private void SpawnLocalGameObjectInstanceClientRPC( ClientRpcParams clientRpcParams = default)
    {
      GameObject Instance =  Instantiate(_newGameObject, _spawnPoint.position, quaternion.identity);
      Instance.transform.SetParent(_currentPlayer.transform);
    }

    
}
