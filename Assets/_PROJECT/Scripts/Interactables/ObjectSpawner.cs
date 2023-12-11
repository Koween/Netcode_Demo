using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;
using TMPro;

public class ObjectSpawner : NetworkBehaviour
{
    //if syncInstance is true the object will appear foar all players else it will appear localy
    [SerializeField] private bool _syncInstance;
    [SerializeField] private bool _setSpawnedObjAsPlayerChild;
    [SerializeField] private GameObject _objPrefab, _currentPlayer;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _messageContainer;
    [SerializeField] private TextMeshProUGUI _messageTextMesh;
    [SerializeField] private string _message = "Press E to interact";

    /*if you delete IsOwner conditional will appear a GameObject Instance for every client conected
    due that code will execute in all clients.
    In this case due that the Owner of this spawner is the server it will just be executed in the server side.
    If you want to let players to execute this besides to take out the conditionl you must use the param owner required
    as false */

    public void OnTriggerEnter(Collider collider)
    {
        if(!collider.CompareTag("Player")) return;
        ulong clientId = collider.GetComponent<NetworkObject>().OwnerClientId;
        ClientRpcParams clientRpcParams = 
            NetworkSessionsManager.Instance.configureClientParams(new ulong[] {clientId});
        SetUIMessageClientRpc(true, clientRpcParams);
    }

    public void OnTriggerExit(Collider collider)
    {
        if(!collider.CompareTag("Player")) return;
        ulong clientId = collider.GetComponent<NetworkObject>().OwnerClientId;
        ClientRpcParams clientRpcParams = 
            NetworkSessionsManager.Instance.configureClientParams(new ulong[] {clientId});
        SetUIMessageClientRpc(false, clientRpcParams);
    }

    public void OnTriggerStay(Collider collider)
    {

        if(!collider.CompareTag("Player")) return;
        _currentPlayer = collider.gameObject;
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(_syncInstance)
            {
                Debug.Log("try sync spawn");
                SpawnOneGameObjectForAllClientsServerRpc();
            }
            else
            {
                ulong clientId = collider.GetComponent<NetworkObject>().OwnerClientId;
                ClientRpcParams clientRpcParams = 
                    NetworkSessionsManager.Instance.configureClientParams(new ulong[] {clientId});
                SpawnGameObjectForSpecificClientRPC(clientRpcParams);

            }
        }
    }

    [ClientRpc]
    private void SetUIMessageClientRpc(bool show, ClientRpcParams clientRpcParams)
    {
        _messageTextMesh.text = _message;
        _messageContainer.SetActive(show);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnOneGameObjectForAllClientsServerRpc()
    {
        Debug.Log(gameObject.GetComponent<NetworkObject>().OwnerClientId);
        NetworkObject networkObject = Instantiate(_objPrefab, _spawnPoint.position, quaternion.identity).GetComponent<NetworkObject>();
        networkObject.Spawn(true);
        if(_setSpawnedObjAsPlayerChild)
        {
            SetWeaponPositionClientRpc(networkObject);
        }
    }

    [ClientRpc]
    private void SetWeaponPositionClientRpc(NetworkObjectReference weaponReference)
    {
        weaponReference.TryGet(out NetworkObject networkObject);
        networkObject.TrySetParent(_currentPlayer);
        Vector3 objPosition = _currentPlayer.transform.position;
        objPosition.x += 0.5f;
        networkObject.transform.position = objPosition;
    }

    //Spawns obj just for the specified clients in the clientParams
    [ClientRpc]
    private void SpawnGameObjectForSpecificClientRPC(ClientRpcParams clientRpcParams = default)
    {
        GameObject Instance = Instantiate(_objPrefab, _spawnPoint.position, quaternion.identity);
        //Instance.transform.SetParent(_currentPlayer.transform);
    }

}
