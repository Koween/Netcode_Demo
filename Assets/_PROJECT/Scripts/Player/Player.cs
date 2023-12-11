using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 180.0f;
    private GameObject weapon;
    public bool IsHoldingObj {get; set;}

    
    void Update()
    {
        if(!IsOwner) return;
        HandleMovement();
        /*if(Input.GetKeyDown(KeyCode.E))
        {
            if(weapon && !IsHoldingObj)
            weapon.GetComponent<NetworkObject>().Despawn(true);
        }*/
    }

    public void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += movement * Time.deltaTime * speed;

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
