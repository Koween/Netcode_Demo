using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 180.0f;

    private Rigidbody rb;

    void Start()
    {
    }

    void Update()
    {
        if(!IsOwner) return;
        HandleMovement();
    }

    public void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Move character based on input
        transform.position += movement * Time.deltaTime * speed;

        // Rotate character towards movement direction
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
