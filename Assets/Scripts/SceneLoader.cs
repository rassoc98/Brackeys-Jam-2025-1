using System;
using System.Collections;
using Audio;
using Game.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static readonly int SceneChanged = Animator.StringToHash("SceneChanged");
    [SerializeField] private GameObject screenTransitionOverlay;

    private void Awake()
    {
        Player.OnPlayerDeath += ReloadScene;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            screenTransitionOverlay.SetActive(false);
        }
        
        OnSceneChanged?.Invoke();
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
        AudioManager.Instance.PlaySound(SceneManager.GetActiveScene().buildIndex == 0 ? "Click" : "End");

        StopAllCoroutines();
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Quit()
    {
        AudioManager.Instance.PlaySound("Click");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator LoadScene(int index)
    {
        screenTransitionOverlay.SetActive(true);
        screenTransitionOverlay.GetComponent<Animator>().SetTrigger(SceneChanged);
        yield return new WaitForSecondsRealtime(1.1f);
        SceneManager.LoadScene(index);
    }
    
    public static event Action OnSceneChanged;
}