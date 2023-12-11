using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handle and shares data about data related to all players like nickNames and conection states
public class NetworkSessionsManager : NetworkBehaviour
{
    public static NetworkSessionsManager Instance {get; private set;}
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        Destroy(this);
    }
    
    public void JoinPlayerAsHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (ulong clientId) => LoadScene("WaitRoom");;
        NetworkManager.Singleton.StartHost();
        
    }

    public void JoinPlayerAssGuest()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void LoadScene(string sceneName)
    {
        
        if(!string.IsNullOrEmpty(sceneName))
        {
            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {sceneName} " +
                        $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }

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
