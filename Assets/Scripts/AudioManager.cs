using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour 
{
    [SerializeField] private AudioMixerGroup[] mixerGroups;
    private List<AudioSource> sources;
    private List<Transform> parents;
    [SerializeField] private int pool;
    private AudioSource source;
    public static AudioManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        sources = new List<AudioSource>();
        parents = new List<Transform>();
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
        for (int i = 0; i < pool; i++)
        {
            GameObject go = new GameObject("Pooled Audio Source");
            AudioSource s = go.AddComponent<AudioSource>();
            go.transform.parent = transform;
            sources.Add(s);
            parents.Add(transform);
        }
    }
    private void Update()
    {
        for (int i = 0;i < pool; i++)
        {
            if (!parents[i])
            {
                parents[i] = transform;
                sources[i].GetComponent<AudioSource>().Stop();
            }
        }
    }
    public AudioSource Play(Transform t,AudioClip c, Mixer mixerGroup = Mixer.None, float volume = 1, bool is3D = false, Vector3 position = new Vector3(), bool loop = false)
    {
        StartCoroutine(PlayAndDestroy(t, c, mixerGroup, volume, is3D, position, loop));
        return source;
    }
    public IEnumerator PlayAndDestroy(Transform t, AudioClip c, Mixer mixerGroup, float volume, bool is3D, Vector3 position, bool loop)
    {
        if (c == null)
        {
            Debug.LogWarning("Null Audio Clip.");
            yield break;
        }
        AudioSource aSource = GetSource(t);
        source = aSource;
        if (aSource != null)
        {
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
                aSource.transform.position = position;
                aSource.spatialBlend = 1;
            }
            aSource.playOnAwake = false;
            aSource.loop = loop;
            aSource.Play();
        }
    }
    private AudioSource GetSource(Transform t)
    {
        AudioSource s;
        for (int i = 0; i < sources.Count; i++)
        {
            s = sources[i];
            if (!s.isPlaying)
            {
                parents[i] = t;
                return s;
            }
        }
        return null;
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
