using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private BoxCollider2D groundCheck;
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelleration;

    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInputs();
    }

    public void HandleInputs()
    {
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) moveInput = 0;
        else if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
        else moveInput = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Grounded()) Jump();
            else 
            {
                StopCoroutine(JumpBuffer());
                StartCoroutine(JumpBuffer());
            }
        }
    }

    private bool Grounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)groundCheck.offset, groundCheck.bounds.size, 0f, Vector2.down, 0.1f);
        return hit;
    }

    private void Jump()
    {
        rb.linearVelocityY = 0f;
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    private IEnumerator JumpBuffer()
    {
        float _jumpBufferTime = 0.2f;
        float _timer = 0f;

        while (_timer <= _jumpBufferTime)
        {
            _timer += Time.deltaTime;
            if (Grounded())
            {
                Jump();
                break;
            }             
            yield return null;
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(moveInput * accelleration * Vector2.right);
        if (Mathf.Abs(rb.linearVelocityX) >= moveSpeed) rb.linearVelocityX = moveSpeed * Mathf.Sign(rb.linearVelocityX);
    }
}
