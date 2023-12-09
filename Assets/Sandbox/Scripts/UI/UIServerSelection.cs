using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class UIServerSelection : MonoBehaviour
{
    [SerializeField] private Button _joinAsHost;
    [SerializeField] private Button _joinAsGuest;

    void Start()
    {
        _joinAsHost.onClick.AddListener(() =>{ HideUI(); NetworkManager.Singleton.StartHost();});
        _joinAsGuest.onClick.AddListener(() =>{ HideUI(); NetworkManager.Singleton.StartClient();});
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

}
