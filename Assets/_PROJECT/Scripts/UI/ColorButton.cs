using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : NetworkBehaviour
{
    [SerializeField] private Color _newColor;
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnPress);

    }

    public void OnPress()
    {
        //Todo: Create logic to change playerColor
    }

}
