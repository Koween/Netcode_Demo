using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] private Button _createRoom, _joinRoom;

    public void Awake()
    {
        _createRoom.onClick.AddListener(OnCreateRoom);
        _joinRoom.onClick.AddListener(OnJoinRoom);
    }

    public void OnCreateRoom()
    {
        Debug.Log("Joining as host");
        NetworkPlayersManager.Instance.JoinPlayerAsHost();
        transform.parent.gameObject.SetActive(false);
    }

    public void OnJoinRoom()
    {
        Debug.Log("Joining as guest");
        NetworkPlayersManager.Instance.JoinPlayerAssGuest();
        transform.parent.gameObject.SetActive(false);
    }
}
