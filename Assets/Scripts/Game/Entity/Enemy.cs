using UnityEngine;

namespace Game.Entity
{
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float[] patrolXPoints = new float[2];
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Trigger chaseTrigger;

    private State _currentState = State.Patrol;
    private float _patrolDirection = 1f;
    private GameObject _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _player = GameObject.FindWithTag("Player");
        chaseTrigger.OnTrigger += () => { _currentState = State.Chase; };

        patrolXPoints[0] += transform.position.x;
        patrolXPoints[1] += transform.position.x;
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        switch (_currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            default: Patrol(); break;
        }
        
        _renderer.flipX = _rb.linearVelocityX < 0f;
    }

    private void Patrol(float deltaTime = 1f)
    {
        if (transform.position.x >= GetRightmostPatrolPoint())
            _patrolDirection = -1f;
        else if (transform.position.x <= GetLeftmostPatrolPoint()) _patrolDirection = 1f;

        _rb.AddForce(_patrolDirection * acceleration * deltaTime * Vector2.right);
        _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -patrolSpeed, patrolSpeed);
    }

    private void Chase(float deltaTime = 1f)
    {
        var direction = Mathf.Sign(_player.transform.position.x - transform.position.x) * Vector3.right;

        _rb.AddForce(acceleration * deltaTime * direction);
        _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -chaseSpeed, chaseSpeed);
    }

    private float GetRightmostPatrolPoint()
    {
        return patrolXPoints[0] > patrolXPoints[1] ? patrolXPoints[0] : patrolXPoints[1];
    }

    private float GetLeftmostPatrolPoint()
    {
        return patrolXPoints[0] < patrolXPoints[1] ? patrolXPoints[0] : patrolXPoints[1];
    }

    private enum State
    {
        Patrol,
        Chase
    }
}
}