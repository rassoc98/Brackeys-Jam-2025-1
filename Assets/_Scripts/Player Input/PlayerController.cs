using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("External References")]
    private Rigidbody2D rb;
    [SerializeField] private InputSystem_Actions playerControls;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMoveInput()
    {
        moveInput = playerControls.Player.Move.ReadValue<float>();
    }

    public void OnJumpPressed()
    {
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveInput * Vector2.right);
        if (Mathf.Abs(rb.linearVelocityX) > moveSpeed) rb.linearVelocityX = moveSpeed * Mathf.Sign(rb.linearVelocityX);
    }
}
