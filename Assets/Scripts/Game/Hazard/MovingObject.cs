using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game.Hazard
{
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
        var start = transform.localPosition;
        var end = start + moveInfo.translation;

        for (var timer = 0f; timer <= moveInfo.transitionTime; timer += Time.deltaTime)
        {
            var lerpValue = timer / moveInfo.transitionTime;
            transform.localPosition = Vector3.Lerp(start, end, lerpValue);

            yield return null;
        }
    }
}

[Serializable]
public class MoveInfo
{
    public Vector3 translation;

    public float transitionTime;
}
}