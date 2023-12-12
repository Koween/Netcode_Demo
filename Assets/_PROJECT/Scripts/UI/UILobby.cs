using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] private Button _createPrivateLobby;
    [SerializeField] private Button _createPublicLobby;
    [SerializeField] private Button _joinLobbyWithCode;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _quickJoinLobby;
    [SerializeField] private TMP_InputField _lobbyNameInputField;
    [SerializeField] private TMP_InputField _lobbyCodeInputField;
    
    public void Awake()
    {
        _createPrivateLobby.onClick.AddListener(() => OnCreateRoom(true));
        _createPublicLobby.onClick.AddListener(() => OnCreateRoom(false));
        _joinLobbyWithCode.onClick.AddListener(OnJoinJoinWithCode);
        _quickJoinLobby.onClick.AddListener(OnQuickJoin);
        _backButton.onClick.AddListener(OnPressBackButton);
    }

    public void OnCreateRoom(bool isPrivate)
    {
        LobbyManager.Instance.CreateLobby(_lobbyNameInputField.text, isPrivate);
    }

    public void OnJoinJoinWithCode()
    {
        LobbyManager.Instance.JoinWithCode(_lobbyCodeInputField.text);
    }

    public void OnQuickJoin()
    {
        LobbyManager.Instance.QuickJoin();
    }

    public void OnPressBackButton()
    {
         LobbyManager.Instance.LeaveLobby();
        NetworkScenesManager.Instance.LoadScene(NetworkScenesManager.GameSecenes.MainMenu);
    }
}
