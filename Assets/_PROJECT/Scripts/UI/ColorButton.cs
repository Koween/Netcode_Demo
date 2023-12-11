using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : NetworkBehaviour
{
    private Color _newColor;
    [SerializeField] private Button _colorButton;
    public void Awake()
    {
        _newColor = GetComponent<Image>().color;
        GetComponent<Button>().onClick.AddListener(OnPress);
    }

    public void OnPress()
    {
        ChangeClientColorServerRpc(NetworkManager.LocalClient.ClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeClientColorServerRpc(ulong clientId)
    {
        NetworkObject playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        playerObject.GetComponent<PlayerAppearance>().ChangePlayerColorClientRpc(_newColor);
        ChangeClientColorClientRpc(playerObject);
    }

    [ClientRpc]
    private void ChangeClientColorClientRpc(NetworkObjectReference player)
    {
        player.TryGet(out NetworkObject playerNobj);
       playerNobj.GetComponent<PlayerAppearance>().ChangePlayerColorClientRpc(_newColor);
    }

}
