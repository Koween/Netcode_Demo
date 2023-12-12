using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayersManager : NetworkBehaviour
{
    [SerializeField] private int _requiredPlayers = 2; 
    public static NetworkPlayersManager Instance {get; private set;}

    
    public int RequiredPlayers { get => _requiredPlayers;}
    private NetworkList<PlayerData> _playerDataNetworkList;
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
    private NetworkVariable<bool> _gameAlreadyStart = new NetworkVariable<bool>();
    public bool GameAlreadyStart { get => _gameAlreadyStart.Value; set => _gameAlreadyStart.Value = value;}
    public int PlayersInGame { get => _playersInGame.Value; }

    public NetworkList<PlayerData> PlayerDataList{get => _playerDataNetworkList;}
    
    public EventHandler OnPlayerDataNetworkListChange, OnFailedToJoinGame, OnTryingToJoinGame;

    //NetworkBehaviour
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            _playerDataNetworkList = new NetworkList<PlayerData>();
            _playerDataNetworkList.OnListChanged += NotifyOnPlayerDataNetworkListChange;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);

    }

    //DataPlayer methods
    public PlayerData GetPlayerDataByClietnId(ulong clientId)
    {
        return _playerDataNetworkList[GetPlayerDataIndexbyClientId(clientId)];
    }

    public int GetPlayerDataIndexbyClientId(ulong clientId)
    {
        for (int i=0; i< _playerDataNetworkList.Count; i++) {
            if (_playerDataNetworkList[i].clientId == clientId) {
                return i;
            }
        }
        return -1;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default) {
        int playerDataIndex = GetPlayerDataIndexbyClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = _playerDataNetworkList[playerDataIndex];
        playerData.playerName = playerName;
        _playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default) {
        int playerDataIndex = GetPlayerDataIndexbyClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = _playerDataNetworkList[playerDataIndex];
        playerData.playerId = playerId;
        _playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void NotifyOnPlayerDataNetworkListChange(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChange?.Invoke(this, EventArgs.Empty);
    }

    //Conection callbacks
     public void JoinPlayerAsHost()
    {
        Debug.Log("Joineng As Host");
        NetworkManager.Singleton.ConnectionApprovalCallback += HandlePlayerJoinRequest;
        NetworkManager.Singleton.OnClientConnectedCallback += CreateDataPlayer;
        NetworkManager.Singleton.OnClientDisconnectCallback += RemoveDataPlayer;
        NetworkManager.Singleton.StartHost();
    }

    public void HandlePlayerJoinRequest
    (NetworkManager.ConnectionApprovalRequest connectionRequest, NetworkManager.ConnectionApprovalResponse connectionResponse)
    {
        Debug.Log("HandleJoinRequest");
        if (SceneManager.GetActiveScene().name != NetworkScenesManager.GameSecenes.WaitRoom.ToString()) {
            connectionResponse.Approved = false;
            connectionResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= NetworkPlayersManager.Instance.RequiredPlayers) {
            connectionResponse.Approved = false;
            connectionResponse.Reason = "Game is full";
            return;
        }

        connectionResponse.Approved = true;
    }

    private void CreateDataPlayer(ulong clientId) {
        Debug.Log("CreateDataPalayer");
        _playerDataNetworkList.Add(new PlayerData {
            clientId = clientId
        });
        SetPlayerNameServerRpc("player: " + clientId);
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    private void RemoveDataPlayer(ulong clientId) {
        for (int i = 0; i < _playerDataNetworkList.Count; i++) {
            PlayerData playerData = _playerDataNetworkList[i];
            if (playerData.clientId == clientId) {
                // Disconnected!
                _playerDataNetworkList.RemoveAt(i);
            }
        }
    }    

    public void JoinPlayerAssClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NotifyOnDisconnectPlayer;
        NetworkManager.Singleton.OnClientConnectedCallback += SetPlayerIdAndNameInServerSide;
        NetworkManager.Singleton.StartClient();
    }

    private void NotifyOnDisconnectPlayer(ulong clientId) {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    private void SetPlayerIdAndNameInServerSide(ulong clientId) {
        //Todo: get name from player script in player prefab and create logic to save player name in playerprefs
        SetPlayerNameServerRpc("player:" + clientId);
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }
    
}
