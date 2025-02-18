using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public class PlayerCatchingHazard : MonoBehaviour
{
    [SerializeField] float transitionTime;
    BoxCollider2D bc;

    // Simple script for mobile hazards that move to try and catch the player after they hit a trigger

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Move(collision.attachedRigidbody.linearVelocityX));
            bc.enabled = false;
        }
    }

    private IEnumerator Move(float speed)
    {
        for(float f = 0; f <= transitionTime; f += Time.deltaTime)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
            yield return null;
        }
    }
}
