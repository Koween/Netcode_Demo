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
        
       // ChangeClientColorClientRpc(NetworkManager.LocalClient.PlayerObject);
       NetworkManager.LocalClient.PlayerObject
       .GetComponent<PlayerAppearance>().ChangePlayerColor(_newColor);
    }
    

/*
    [ClientRpc]
    private void ChangeClientColorClientRpc(NetworkObjectReference player)
    {
        player.TryGet(out NetworkObject playerNObj);
        playerNObj.GetComponent<PlayerAppearance>().ChangePlayerColor(_newColor);
    }
*/
}
