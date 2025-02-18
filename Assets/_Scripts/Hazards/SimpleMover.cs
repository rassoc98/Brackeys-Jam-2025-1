using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public class SimpleMover : MonoBehaviour
{
    [SerializeField] private MoveInfo[] moveInfos;
    BoxCollider2D bc;

    // Simple script for mobile hazards that move once after a trigger
    // usage note - uses GetComponent<BoxCollider2D>() to find the trigger, so make sure its above the collision box.

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
        foreach (var moveInfo in moveInfos)
        {
            Vector3 start = transform.localPosition;
            Vector3 end = moveInfo.targetPosition.localPosition;

            for (float f = 0; f <= moveInfo.transitionTime; f += Time.deltaTime)
            {
                float lerpValue = f / moveInfo.transitionTime;
                transform.localPosition = Vector3.Lerp(start, end, lerpValue);

                yield return null;
            }
        }
    }
}

[System.Serializable]
public class MoveInfo
{
    public Transform targetPosition;
    public float transitionTime;
}
