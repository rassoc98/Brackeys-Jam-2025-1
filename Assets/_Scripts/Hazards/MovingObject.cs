using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private MoveInfo[] moveInfos;
    [SerializeField] private Trigger trigger;

    // Simple script for mobile hazards that move once after a trigger
    // usage note - uses GetComponent<BoxCollider2D>() to find the trigger, so make sure its above the collision box.

    private void Awake()
    {
        trigger.OnTrigger += () => { StartCoroutine(ExecuteMovements()); };
    }

    private IEnumerator ExecuteMovements()
    {
        return moveInfos.Select(moveInfo => StartCoroutine(Move(moveInfo))).GetEnumerator();
    }

    private IEnumerator Move(MoveInfo moveInfo)
    {
        Vector3 start = transform.localPosition;
        Vector3 end   = start + moveInfo.translation;
        
        for (float timer = 0f; timer <= moveInfo.transitionTime; timer += Time.deltaTime)
        {
            float lerpValue = timer / moveInfo.transitionTime;
            transform.localPosition = Vector3.Lerp(start, end, lerpValue);

            yield return null;
        }
    }
}

[System.Serializable]
public class MoveInfo
{
    [FormerlySerializedAs("positionDelta")] public Vector3 translation;
    public float transitionTime;
}
