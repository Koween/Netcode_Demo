using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//Todo: change name to NetworkSceneManager
public class NetworkScenesManager : NetworkBehaviour
{
    public enum GameSecenes
    {
        WaitRoom,
        Lobby,
        GameScene,
        MainMenu
    }

    public static NetworkScenesManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }


    public void LoadNetworkScene(GameSecenes scene)
    {


        var status = NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {scene} " +
                    $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }

    }

    public void LoadScene(GameSecenes scene)
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(scene.ToString());
        Destroy(NetworkManager.Singleton.gameObject);
        Destroy(NetworkPlayersManager.Instance.gameObject);
        Destroy(gameObject);
    }

    //Todo: Crear clase para gestionar params
    public ClientRpcParams configureClientParams(ulong[] clientsId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            { TargetClientIds = clientsId }
        };
        return clientRpcParams;
    }

}
