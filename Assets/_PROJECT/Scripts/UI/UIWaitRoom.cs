using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitRoom : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;

    public void Awake()
    {
        _startGameButton.onClick.AddListener(LoadGameScene);
    }

   
    private void LoadGameScene()
    {
        NetworkSessionsManager.Instance.LoadScene("GameScene");
        
    }
}
