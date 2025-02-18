using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static readonly int SceneChanged = Animator.StringToHash("SceneChanged");
    private static readonly int SceneStart = Animator.StringToHash("SceneStart");
    [SerializeField] private GameObject screenTransitionOverlay;

    private void Awake()
    {
        Player.OnPlayerDeath += ReloadScene;
    }

    private void Start()
    {
        screenTransitionOverlay.GetComponent<Animator>().SetTrigger(SceneStart);
    }

    private void OnDestroy()
    {
        Player.OnPlayerDeath -= ReloadScene;
    }

    public void ReloadScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadScene(int index)
    {
        screenTransitionOverlay.GetComponent<Animator>().SetTrigger(SceneChanged);
        yield return new WaitForSecondsRealtime(1.1f);
        SceneManager.LoadScene(index);
    }
}