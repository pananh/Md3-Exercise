using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float velX = Input.GetAxis("Horizontal");
        float velZ = Input.GetAxis("Vertical");

        // Set parameters for Blend Tree
        animator.SetFloat("VelX", velX);
        animator.SetFloat("VelZ", velZ);

        // Move character
        Vector3 move = new Vector3(velX, 0, velZ) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }
}
