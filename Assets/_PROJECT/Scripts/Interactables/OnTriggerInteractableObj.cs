using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class OnTriggerInteractableObj : NetworkBehaviour
{
    [SerializeField] private GameObject _messageContainer;
    [SerializeField] private TextMeshProUGUI _messageTextMesh;
    [SerializeField] private string _message = "Press E to interact";
    
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

    [ClientRpc]
    private void SetUIMessageClientRpc(bool show, ClientRpcParams clientRpcParams)
    {
        _messageTextMesh.text = _message;
        _messageContainer.SetActive(show);
    }
}
