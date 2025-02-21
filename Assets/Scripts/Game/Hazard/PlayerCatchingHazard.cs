using System.Collections;
using UnityEngine;

namespace Game.Hazard
{
public class PlayerCatchingHazard : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private Trigger trigger;

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
            if (_player == null) break;

            var direction = Mathf.Sign(_player.transform.position.x - transform.position.x) * Vector3.right;
            var speed = Mathf.Abs(playerRb.linearVelocityX);
            transform.Translate(speed * Time.deltaTime * direction);
            yield return null;
        }
    }
}
}