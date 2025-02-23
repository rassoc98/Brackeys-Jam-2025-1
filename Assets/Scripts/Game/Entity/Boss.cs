using System.Collections;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entity
{
public class Boss : MonoBehaviour
{
    public int Health {get; private set;}
    
    [SerializeField] private int maxHealth;
    [SerializeField] private float spawnTimeout;
    [SerializeField] private float buttonTimeout;
    [SerializeField] private float initialDelay;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] buttons;

    private float _spawnTimer;
    private float _buttonTimer;
    private int _currentButtonIndex;
    private bool _firstButton = true;
    private bool _isDead;

    private void Awake()
    {
        Health = maxHealth;
        foreach (var button in buttons) button.SetActive(false);
        _spawnTimer -= initialDelay;
        _buttonTimer -= initialDelay;
    }

    private void Update()
    {
        if (_isDead) return;
        
        _spawnTimer += Time.deltaTime;
        _buttonTimer += Time.deltaTime;

        if (Health <= 0)
        {
            StartCoroutine(EndScene());
            _isDead = true;
        }
        
        if (_buttonTimer >= buttonTimeout && _currentButtonIndex < buttons.Length)
        {
            buttons[_currentButtonIndex].SetActive(true);
            _currentButtonIndex++;
            _buttonTimer = 0f;
            
            if (_firstButton)
            {
                StartCoroutine(FindFirstObjectByType<DialogueBox>().PlayNextConversation());
                _firstButton = false;
            }
        }
        
        if (_spawnTimer < spawnTimeout) return;
        
        Instantiate(obstacles[Random.Range(0, obstacles.Length)], Vector3.zero, Quaternion.identity);
        _spawnTimer = 0f;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    
    private IEnumerator EndScene()
    {
        //yield return FindFirstObjectByType<DialogueBox>().PlayNextConversation();
        var player = GameObject.FindWithTag("Player");
        Destroy(player);
        FindFirstObjectByType<SceneLoader>().LoadNextScene();
        yield return null;
    }
}
}
