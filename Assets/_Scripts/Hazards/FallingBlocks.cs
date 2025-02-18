using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    Rigidbody2D rb;

    // A script designed to make falling floors

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D otherRB = collision.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocityY = otherRB.linearVelocityY;
            rb.gravityScale = otherRB.gravityScale;

            foreach (BoxCollider2D bc in gameObject.GetComponents<BoxCollider2D>())
            {
                bc.enabled = false;
            }
        }
    }
}
