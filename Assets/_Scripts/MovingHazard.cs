using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public class MovingHazard : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] float transitionTime;
    BoxCollider2D bc;

    // Simple script for mobile hazards that move once after a trigger

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Move());
            bc.enabled = false;
        }
    }

    private IEnumerator Move()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosition.position;

        for(float f = 0; f <= transitionTime; f += Time.deltaTime)
        {
            float lerpValue = f / transitionTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpValue);
            yield return null;
        }
    }
}
