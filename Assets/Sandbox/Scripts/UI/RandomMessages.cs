using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RandomMessages : NetworkBehaviour
{
    [SerializeField] private List<string> _messages;
    [SerializeField] private int _messagesDelay;
    [SerializeField] private TextMeshProUGUI _text;
    private float _timeCounter;

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        _timeCounter += Time.deltaTime;
        if(_messagesDelay <= _timeCounter)
        {
            _timeCounter = 0;
            ShowMessageClientRPC(_messages[Random.Range(0, _messages.Count)]);
        }
    }

    [ClientRpc]
    private void ShowMessageClientRPC(string message)
    {
        _text.text = message;
    }
}
