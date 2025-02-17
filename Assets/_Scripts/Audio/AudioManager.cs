using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// The Audio Manager is used to play audio (both SFX and music, however, music is not implemented yet).
///
/// The Audio Manager stores an array of all the game's sounds, each sound has a name that'll be used for playing it,
/// Sound could be played by using AudioManager.Instance.Play(soundName, [optional] position, [optional] volume).
/// </summary>

// TODO: Music

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    
    [SerializeField] private Sound[] sounds;
    
    private ObjectPoolManager _objectPoolManager;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        
        _objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    public IEnumerator Play(string soundName, Vector3 position = new(), float volume = 1.0f) 
    {
        var soundObject = _objectPoolManager.Spawn(ObjectPoolManager.ObjectType.SoundObject);
        soundObject.transform.position = position;

        var audioSource = soundObject.GetComponent<AudioSource>();
        var sound = Array.Find(sounds, sound => sound.name == soundName);
        
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume * volume;
        audioSource.pitch = sound.pitch;
        audioSource.Play();

        var clipLength = audioSource.clip.length;
        yield return new WaitForSeconds(clipLength);
        
        _objectPoolManager.Deactivate(soundObject.GetComponent<PoolObject>());
    }
}

// Sound information
[Serializable]
internal class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.2f, 5f)]
    public float pitch;
}