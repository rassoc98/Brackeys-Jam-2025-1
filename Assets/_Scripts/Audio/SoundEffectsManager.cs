using System.Collections;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour {
    public static SoundEffectsManager instance;
    [SerializeField] private GameObject SFXObject;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public IEnumerator PlaySFXClip(AudioClip audioClip, Vector3 pos, float volume = 0.5f) 
    {
        var audioObject = ObjPoolManager.SpawnObject(SFXObject);
        audioObject.transform.parent = transform;

        var audioSource = audioObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        var clipLength = audioSource.clip.length;
        yield return new WaitForSeconds(clipLength);
        ObjPoolManager.RecycleObject(audioObject);
    }
}