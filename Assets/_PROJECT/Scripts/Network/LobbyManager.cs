using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Lobby joinedLobby;
    public static LobbyManager Instance {get; private set;}
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            InitializeUnityAutentication();
            return;
        }
        else
        DontDestroyOnLoad(gameObject);

    }
    
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
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, NetworkPlayersManager.Instance.RequiredPlayers, 
            new CreateLobbyOptions {IsPrivate = isPrivate});
            NetworkPlayersManager.Instance.JoinPlayerAsHost();
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
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            NetworkPlayersManager.Instance.JoinPlayerAssGuest();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
