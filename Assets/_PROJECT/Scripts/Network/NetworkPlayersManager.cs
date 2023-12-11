using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayersManager : NetworkBehaviour
{
    [SerializeField] private int _requiredPlayers = 2; 
    public static NetworkPlayersManager Instance {get; private set;}

    public int RequiredPlayers { get => _requiredPlayers;}

    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();

    public int PlayersInGame { get => _playersInGame.Value; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            
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
            if(IsServer) _playersInGame.Value--;
        }; 
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
