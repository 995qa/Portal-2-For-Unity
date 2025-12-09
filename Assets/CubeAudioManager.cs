using UnityEngine;

public class CubeAudioManager : MonoBehaviour 
{
    private float vel;
    private float prev;
    private Rigidbody rb;
    [SerializeField] private AudioClip[] softHard;
    [SerializeField] private float maxVel = 5f;
    [SerializeField] private float ratio = 0.2f;
    [SerializeField] private float volume = 0.4f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        prev = vel;
        vel = rb.velocity.magnitude;
    }
    void OnTriggerEnter(Collider col)
    {
        float hardVolume = Mathf.Min(prev, maxVel)/maxVel;
        float softVolume = (1 - hardVolume) * ratio;
        AudioManager.Instance.Play(transform, softHard[0], AudioManager.Mixer.SFX, softVolume * volume, true, transform.position, false);
        AudioManager.Instance.Play(transform, softHard[1], AudioManager.Mixer.SFX, hardVolume * volume, true, transform.position, false);
    }
}
