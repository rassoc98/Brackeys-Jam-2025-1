using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private BoxCollider2D groundCheck;
    private Rigidbody2D _rb;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private KeyCode[] jumpKeys;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float friction = 0.5f;

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
        Vector2 direction = Input.GetAxisRaw("Horizontal") * Vector2.right;
        //moved handling decelleration away from the material, so we now slide down walls instead of getting stuck on them

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _rb.AddForce(acceleration * direction);
            _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -moveSpeed, moveSpeed);
        }
        else _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0, friction); 

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
        RaycastHit2D hit = Physics2D.BoxCast(
            origin: transform.position + (Vector3)groundCheck.offset,
            size: groundCheck.bounds.size,
            angle: 0f,
            direction: Vector2.down,
            distance: 0.1f
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
        float timer = 0f;

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
