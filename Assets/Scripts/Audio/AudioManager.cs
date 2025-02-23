using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
/// <summary>
///     The Audio Manager is used to play audio (both SFX and music, however, music is not implemented yet).
///     The Audio Manager stores an array of all the game's sounds, each sound has a name that'll be used for playing it,
///     Sound could be played by using AudioManager.Instance.PlaySound(soundName, [optional] position, [optional] volume).
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Audio[] sounds;
    [SerializeField] private SoundTrack soundtrack;
    [SerializeField] private GameObject soundObjectPrefab;
    [SerializeField] private AudioSource musicSource;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
        SceneLoader.OnSceneChanged += UpdateMusic;
        musicSource.loop = true;
        musicSource.clip =
            SceneManager.GetActiveScene().buildIndex == 0 ?
            soundtrack["Title Screen"].audio.clip :
            soundtrack["Gameplay"].audio.clip;
    }

    private void Start()
    {
        UpdateMusic();
        musicSource.Play();
    }
    
    public void PlaySound(string soundName, Vector3 position = new(), float volume = 0.5f)
    {
        StartCoroutine(Play(soundName, position, volume));
    }
    
    public void PlaySoundOnce(string soundName, float volume = 0.5f)
    {
        var sound = Array.Find(sounds, sound => sound.name == soundName);

        _audioSource.clip = sound.clip;
        _audioSource.volume = (sound.volume + volume) / 2f;
        _audioSource.pitch = sound.pitch;
        _audioSource.Play();
    }

    private void UpdateMusic()
    {
        var currentScene  = SceneManager.GetActiveScene();
        if (currentScene.buildIndex != 1) return;
        
        musicSource.Stop();
        musicSource.clip = soundtrack["Gameplay"].audio.clip;
        musicSource.Play();
    }

    private IEnumerator Play(string soundName, Vector3 position = new(), float volume = 0.5f)
    {
        var soundObject = Instantiate(soundObjectPrefab, position, Quaternion.identity);
        soundObject.transform.position = position;

        var audioSource = soundObject.GetComponent<AudioSource>();
        var sound = Array.Find(sounds, sound => sound.name == soundName);

        audioSource.clip = sound.clip;
        audioSource.volume = (sound.volume + volume) / 2f;
        audioSource.pitch = sound.pitch;
        audioSource.Play();

        var clipLength = audioSource.clip.length;
        yield return new WaitForSeconds(clipLength);

        Destroy(soundObject);
    }
}

[Serializable]
internal class SoundTrack
{
    public Music[] soundTrack;
    public Music this[string sceneName] => Array.Find(soundTrack, music => music.sceneName == sceneName);
}

[Serializable]
internal class Music
{
    public string sceneName;
    public Audio audio;
}

// Audio information
[Serializable]
internal class Audio
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume;

    [Range(0.2f, 5f)] public float pitch;
}
}