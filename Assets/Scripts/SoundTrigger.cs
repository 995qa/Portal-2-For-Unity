using UnityEngine;

public class SoundTrigger : TriggerPrimitive
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool is2D;
    [SerializeField, Range(0,1)] private float volume;
    [SerializeField] private bool loop;
    [SerializeField] private AudioManager.Mixer mixerGroup;
    public override void Enter()
    {
        AudioManager.Instance.Play(transform, audioClip, mixerGroup, volume,!is2D,transform.position, loop);
    }
}
