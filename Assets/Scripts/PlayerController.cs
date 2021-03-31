using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public float jumpHeight = 3.0f;
    public float playerSpeed = 2.0f;
    public float gravityValue = -9.81f;

    private Vector3 playerVelocity;
    private Vector3 lastPosition;
    private bool groundedPlayer;

    void Update()
    {
        Debug.Log(groundedPlayer);
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * gravityValue);
            animator.SetTrigger("Jump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Speed is the distance between my current position - last position over time
        float speed = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        animator.SetFloat("Speed", speed);
        lastPosition = this.transform.position;
    }
}
