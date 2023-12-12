using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Lobby _joinedLobby;
    public static LobbyManager Instance {get; private set;}
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUnityAutentication();
            return;
        }
        Destroy(gameObject);
    }

    public Lobby JoinedLobby {get => _joinedLobby;}
    
    private async void InitializeUnityAutentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initOptions = new InitializationOptions();
            initOptions.SetProfile(Random.Range(0, 1000).ToString());
            await UnityServices.InitializeAsync(initOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, NetworkPlayersManager.Instance.RequiredPlayers, 
            new CreateLobbyOptions {IsPrivate = isPrivate});
            NetworkPlayersManager.Instance.JoinPlayerAsHost();
            NetworkScenesManager.Instance.LoadNetworkScene(NetworkScenesManager.GameSecenes.WaitRoom);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoin()
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            NetworkPlayersManager.Instance.JoinPlayerAssClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinWithCode(string code)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            NetworkPlayersManager.Instance.JoinPlayerAssClient();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private bool IsLobbyHost()
    {
        return JoinedLobby != null && JoinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void DeleteLobby() {
        if (_joinedLobby != null) {
            try {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);

                _joinedLobby = null;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby() {
        if (_joinedLobby != null) {
            try {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                _joinedLobby = null;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId) {
        if (IsLobbyHost()) {
            try {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

}
