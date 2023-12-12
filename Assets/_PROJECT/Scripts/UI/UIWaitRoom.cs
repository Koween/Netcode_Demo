using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitRoom : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _joindePlayersText;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private Button _playGameButton;
    [SerializeField] private Button _backToMainMenuButton;

    void Awake()
    {
        _playGameButton.onClick.AddListener(LoadGameSceneServerRpc);
        _playGameButton.interactable = false;
        _backToMainMenuButton.onClick.AddListener(OnPressBackButton);
        string lobbyName = LobbyManager.Instance.JoinedLobby.Name;
        string lobbyCode = LobbyManager.Instance.JoinedLobby.LobbyCode;
        _lobbyNameText.text = $"name: {lobbyName}";
        _lobbyCodeText.text = $"code: {lobbyCode}";
    }

    private void OnPressBackButton()
    {
        NetworkScenesManager.Instance.LoadScene(NetworkScenesManager.GameSecenes.MainMenu);
    }

   [ServerRpc(RequireOwnership = false)]
    private void LoadGameSceneServerRpc()
    {
        NetworkScenesManager.Instance.LoadNetworkScene(NetworkScenesManager.GameSecenes.GameScene);
        NetworkPlayersManager.Instance.GameAlreadyStart = true;
        gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if(IsOwner)
        UpdateConectedPlayersTextServerRpc();
    }

    [ServerRpc]
    private void UpdateConectedPlayersTextServerRpc()
    {
        UpdateConectedPlayersTextClientRpc();
    }

    [ClientRpc]
    private void UpdateConectedPlayersTextClientRpc()
    {
        int currentPlayers = NetworkPlayersManager.Instance.PlayerDataList.Count;
        int requiredPlayers = NetworkPlayersManager.Instance.RequiredPlayers;
        _joindePlayersText.text = $"Joined players {currentPlayers}/{requiredPlayers}";
        if(currentPlayers == requiredPlayers && IsOwner)
        {
            _playGameButton.interactable = true;
        }
    }
}
