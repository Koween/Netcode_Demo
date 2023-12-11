using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAppearance : NetworkBehaviour
{
    [SerializeField] private MeshRenderer _playerMeshRender;

    private Material _material;

    public void ChangePlayerColor(Color newColor)
    {
        _material = new Material(_playerMeshRender.material);
        _playerMeshRender.material = _material;
        _material.color = newColor;
    }
}
