using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitRoom : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _joindePlayersText;
    [SerializeField] private Button _playGameButton;

    void Awake()
    {
        _playGameButton.onClick.AddListener(LoadGameSceneServerRpc);
        _playGameButton.interactable = false;
    }

   [ServerRpc(RequireOwnership = false)]
    private void LoadGameSceneServerRpc()
    {
        NetworkScenesManager.Instance.LoadScene("GameScene");
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
        int currentPlayers = NetworkPlayersManager.Instance.PlayersInGame;
        int requiredPlayers = NetworkPlayersManager.Instance.RequiredPlayers;
        _joindePlayersText.text = $"Joined players {currentPlayers}/{requiredPlayers}";
        if(currentPlayers == requiredPlayers && IsOwner)
        {
            _playGameButton.interactable = true;
        }
    }
}
