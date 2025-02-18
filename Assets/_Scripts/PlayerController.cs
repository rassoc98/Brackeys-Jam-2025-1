using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("External References")] [SerializeField]
    private BoxCollider2D groundCheck;

    [Header("Movement")] [SerializeField] private float jumpForce;

    [SerializeField] private KeyCode[] jumpKeys;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float friction = 0.5f;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
        //moved handling decelleration away from the material, so we now slide down walls instead of getting stuck on them

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

    private bool IsGrounded()
    {
        var hit = Physics2D.BoxCast(
            transform.position + (Vector3)groundCheck.offset,
            groundCheck.bounds.size,
            0f,
            Vector2.down,
            0.1f
        );

        if (hit.collider == null) return false;

        return hit.collider.gameObject.CompareTag("Floor");
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