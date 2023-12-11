using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

//Todo: change name to NetworkSceneManager
public class NetworkScenesManager : NetworkBehaviour
{
    public static NetworkScenesManager Instance {get; private set;}
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
