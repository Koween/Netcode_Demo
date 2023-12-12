using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Player : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 180.0f;
    private GameObject weapon;
    public bool IsHoldingObj {get; set;}
    float _horizontalMovement;
    float _verticalMovement;
    [SerializeField] Animator _animator;
    
    void Update()
    {
        if(!IsOwner) return;
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
        HandleMovement();
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if(_verticalMovement != 0|| _horizontalMovement != 0)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public void HandleMovement()
    {
        Vector3 movement = new Vector3(_horizontalMovement, 0, _verticalMovement).normalized;

        transform.position += movement * Time.deltaTime * speed;

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
