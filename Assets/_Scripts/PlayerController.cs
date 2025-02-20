using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");

    [Header("External References")] [SerializeField]
    private BoxCollider2D groundCheck;

    [Header("Movement")] [SerializeField] private float jumpForce;

    [SerializeField] private KeyCode[] jumpKeys;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float friction = 0.5f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        var direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
        //moved handling deceleration away from the material, so we now slide down walls instead of getting stuck on them

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _rb.AddForce(acceleration * direction);
            _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -moveSpeed, moveSpeed);
        }
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0, friction);
        }

        if (!jumpKeys.Any(Input.GetKey)) return;

        if (IsGrounded())
        {
            Jump();
        }
        else
        {
            StopCoroutine(JumpWhenGrounded(0.2f));
            StartCoroutine(JumpWhenGrounded(0.2f));
        }
    }
    
    private void HandleAnimation()
    {
        var isMoving  = IsGrounded() && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f;
        var isFalling = !IsGrounded() && _rb.linearVelocityY < -0.01f;
        var isJumping = !IsGrounded() && _rb.linearVelocityY > 0.01f;
        
        _animator.SetBool(IsMoving, isMoving);
        _animator.SetBool(IsJumping, isJumping);
        _animator.SetBool(IsFalling, isFalling);
        
        _spriteRenderer.flipX = (int) Mathf.Sign(Input.GetAxisRaw("Horizontal")) != 1;
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.BoxCast(
            transform.position + (Vector3) groundCheck.offset,
            groundCheck.bounds.size,
            0f,
            Vector2.down,
            1.0f
        );

        return hit.collider != null && hit.collider.gameObject.CompareTag("Floor");
    }

    private void Jump()
    {
        _rb.linearVelocityY = 0f;
        _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    private IEnumerator JumpWhenGrounded(float timeWindowSeconds = Mathf.Infinity)
    {
        var timer = 0f;

        while (timer <= timeWindowSeconds)
        {
            timer += Time.deltaTime;
            if (IsGrounded())
            {
                Jump();
                break;
            }

            yield return null;
        }
    }
}