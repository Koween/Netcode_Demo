using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;

    // Update is called once per frame
    void Awake()
    {
        _startHostButton.onClick.AddListener(() => {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        _startHostButton.onClick.AddListener(() => {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
