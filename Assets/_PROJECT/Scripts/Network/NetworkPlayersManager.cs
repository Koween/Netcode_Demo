using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayersManager : NetworkBehaviour
{
    [SerializeField] private int _requiredPlayers = 2; 
    public static NetworkPlayersManager Instance {get; private set;}

    public int RequiredPlayers { get => _requiredPlayers;}

    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
    private NetworkVariable<bool> _gameAlreadyStart = new NetworkVariable<bool>();
    public bool GameAlreadyStart { get => _gameAlreadyStart.Value; set => _gameAlreadyStart.Value = value;}
    public int PlayersInGame { get => _playersInGame.Value; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            return;
            
        }
        Destroy(this);

    }

    void Start()
    {
        HandlePlayersConections();
    }
 
    public void HandlePlayersConections()
    {
        
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if(IsServer) _playersInGame.Value++;
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if(IsServer) OnPlayerDisconect(id); 
        }; 
    }

    public void OnPlayerDisconect(ulong clientId)
    {
        _playersInGame.Value--;
        if(_gameAlreadyStart.Value || clientId == NetworkManager.ServerClientId) 
        {
            //Todo: show UI message ("conection error")
            NetworkScenesManager.Instance.DisconectAllPlayers();
        }
    }

    public void JoinPlayerAsHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += 
        (ulong clientId) => NetworkScenesManager.Instance.LoadScene("WaitRoom");;
        NetworkManager.Singleton.StartHost();
    }

    public void JoinPlayerAssGuest()
    {
        NetworkManager.Singleton.StartClient();
    }

}
