using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _startMultiplayer, _startSinglePlayer;

    private void Awake()
    {
        _startMultiplayer.onClick.AddListener(OnPreesMultiplayer);
        _startSinglePlayer.onClick.AddListener(OnPreesSingleplayer);
    }

    public void OnPreesMultiplayer()
    {
        SceneManager.LoadScene("Lobby");    
    }

    public void OnPreesSingleplayer()
    {
        SceneManager.LoadScene("GameScene");    
    }
}
