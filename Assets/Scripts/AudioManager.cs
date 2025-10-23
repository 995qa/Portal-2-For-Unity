using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour 
{
    [SerializeField] private AudioMixerGroup[] mixerGroups;
    public static AudioManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one " + this.name + ", ya chump");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Play(Transform t,AudioClip c, Mixer mixerGroup = Mixer.None, float volume = 1, bool is3D = false, Vector3 position = new Vector3(), bool loop = false)
    {
        StartCoroutine(PlayAndDestroy(t, c, mixerGroup, volume, is3D, position, loop));
    }
    public IEnumerator PlayAndDestroy(Transform t, AudioClip c, Mixer mixerGroup, float volume, bool is3D, Vector3 position, bool loop)
    {
        if (c == null)
        {
            Debug.LogWarning("AudioRitual: Null clip passed to coroutine.");
            yield break;
        }
        GameObject tempGO = new GameObject("TempAudioSource");
        tempGO.transform.parent = t;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = c;
        aSource.volume = 1;
        if (mixerGroup != Mixer.None)
        {
            aSource.outputAudioMixerGroup = mixerGroups[(int)mixerGroup];
            //aSource.volume *= mixerGroups[(int)mixerGroup];
        }
        aSource.volume *= volume;
        aSource.spatialBlend = 0;
        if (is3D)
        {
            tempGO.transform.position = position;
            aSource.spatialBlend = 1;
        }
        aSource.playOnAwake = false;
        aSource.loop = loop;
        aSource.Play();
        if (!loop)
        {
            yield return new WaitForSeconds(c.length);
            Destroy(tempGO);
        }
    }
    public enum Mixer
    {
        None = -1,
        UI = 0,
        Ambience = 1,
        Music = 2,
        SFX = 3,
        Dialogue = 4,
    }
}
