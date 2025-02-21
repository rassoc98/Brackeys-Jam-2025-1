using UnityEngine;

namespace Game
{
[RequireComponent(typeof(Collider2D))]
public class LevelEnd : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        FindFirstObjectByType<SceneLoader>().LoadNextScene();
    }
}
}
