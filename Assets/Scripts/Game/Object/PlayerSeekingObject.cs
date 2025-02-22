using System.Collections;
using UnityEngine;

namespace Game.Object
{
public class PlayerSeekingObject : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private Trigger trigger;
    [SerializeField] private bool xAxis, yAxis;

    private GameObject _player;

    // Simple script for mobile hazards that move to try and catch the player after they hit a trigger

    private void Awake()
    {
        trigger.OnTrigger += StartMoving;
        _player = GameObject.FindWithTag("Player");
    }

    private void StartMoving()
    {
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var initialPosition = transform.position;
        var playerRb = _player.GetComponent<Rigidbody2D>();

        while (Vector3.Distance(initialPosition, transform.position) < maxDistance)
        {
            Move(playerRb);
            yield return null;
        }
    }

    private void Move(Rigidbody2D playerRigidBody)
    {
        if (xAxis)
        {
            var direction = Mathf.Sign(_player.transform.position.x - transform.position.x) * Vector3.right;
            var speed = Mathf.Abs(playerRigidBody.linearVelocityX);
            transform.Translate(speed * Time.deltaTime * direction);
        }

        if (yAxis)
        {
            var direction = Mathf.Sign(_player.transform.position.y - transform.position.y) * Vector3.up;
            var speed = Mathf.Abs(playerRigidBody.linearVelocityY);
            transform.Translate(speed * Time.deltaTime * direction);
        }
    }
}
}