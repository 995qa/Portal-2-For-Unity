using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController3D : MonoBehaviour 
{
    [SerializeField] private Transform cam;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float friction;
    [SerializeField] private AudioClip[] wAudioClips;
    [SerializeField] private AudioClip[] bAudioClips;
    [SerializeField] private float vol;
    private Rigidbody rb;
    private Transform player;
    private float angle;
    private bool grounded;
    private Material currOn;
    private bool playing;
    [SerializeField] private float dur;
    private bool move;
    private List<AudioClip>[] audioClips;
    public static PlayerController3D Instance
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
        player = transform;
        rb = GetComponent<Rigidbody>();
        audioClips = new List<AudioClip>[2];
        audioClips[0] = wAudioClips.ToList();
        audioClips[1] = bAudioClips.ToList();
    }
    void Update () 
	{
#if UNITY_EDITOR
        angle = cam.rotation.eulerAngles.x;
        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle - (Input.GetAxis("Mouse Y") * lookSpeed * 2) < -90)
        {
            cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else if (angle - (Input.GetAxis("Mouse Y") * lookSpeed * 2) > 90)
        {
            cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                player.rotation = Quaternion.Euler(
                    new Vector3(
                        player.rotation.eulerAngles.x,
                        player.rotation.eulerAngles.y + (Input.GetAxis("Mouse X") * lookSpeed * 2),
                        player.rotation.eulerAngles.z
                        )
                    );
                cam.rotation = Quaternion.Euler(
                    new Vector3(
                        cam.rotation.eulerAngles.x - (Input.GetAxis("Mouse Y") * lookSpeed * 2),
                        cam.rotation.eulerAngles.y,
                        cam.rotation.eulerAngles.z
                        )
                    );
            }
        }
#else
        
        if(Input.touchCount != 0)
        {
            angle = cam.rotation.eulerAngles.x;
            if (angle > 180)
            {
                angle = angle - 360;
            }
            if (angle - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5) < -90)
            {
                cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
            }
            else if (angle - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5) > 90)
            {
                cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
            }
            else
            {
                if(Input.GetTouch(0).position != new Vector2(0, 0))
                {
                    player.rotation = Quaternion.Euler(
                        new Vector3(
                            player.rotation.eulerAngles.x,
                            player.rotation.eulerAngles.y + (Input.GetTouch(0).deltaPosition.x * lookSpeed / 5),
                            player.rotation.eulerAngles.z
                            )
                        );
                    cam.rotation = Quaternion.Euler(
                        new Vector3(
                            cam.rotation.eulerAngles.x - (Input.GetTouch(0).deltaPosition.y * lookSpeed / 5),
                            cam.rotation.eulerAngles.y,
                            cam.rotation.eulerAngles.z
                            )
                        );
                }
            }
        }
#endif

        float playerAngle = UnityEngine.N3DS.GamePad.CirclePadPro.x * lookSpeed;
        float camAngle = UnityEngine.N3DS.GamePad.CirclePadPro.y * lookSpeed;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerAngle += lookSpeed / 5;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerAngle -= lookSpeed / 5;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            camAngle += lookSpeed / 5;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            camAngle -= lookSpeed / 5;
        }
#endif
        rb.freezeRotation = false;
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y + playerAngle, player.rotation.eulerAngles.z);
        rb.freezeRotation = true;
        angle = cam.rotation.eulerAngles.x;
        if (angle > 180)
        {
            angle = angle - 360;
        }
        if (angle - camAngle < -90)
        {
            cam.rotation = Quaternion.Euler(-90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else if (angle - camAngle > 90)
        {
            cam.rotation = Quaternion.Euler(90f, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        else
        {
            cam.rotation = Quaternion.Euler(cam.rotation.eulerAngles.x - camAngle, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);
        }
        Vector2 horVel = new Vector2(rb.velocity.x, rb.velocity.z);
        float forwardForce = UnityEngine.N3DS.GamePad.CirclePad.y * Time.deltaTime;
        float sidewaysForce = UnityEngine.N3DS.GamePad.CirclePad.x * Time.deltaTime;
        move = false;
        if (UnityEngine.N3DS.GamePad.CirclePad.magnitude > 0)
        {
            move = true;
        }
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W))
        {
            forwardForce += 1 * Time.deltaTime;
            move = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forwardForce -= 1 * Time.deltaTime;
            move = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            sidewaysForce += 1 * Time.deltaTime;
            move = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            sidewaysForce -= 1 * Time.deltaTime;
            move = true;
        }
#endif
        if (forwardForce!=0 || sidewaysForce != 0)
        {
            PortalGunAnimator.Instance.animate = true;
        }
        else
        {
            PortalGunAnimator.Instance.animate = false;
        }
        if (horVel.magnitude < maxSpeed)
        {
            rb.velocity += transform.forward * forwardForce * accelSpeed;
            rb.velocity += transform.right * sidewaysForce * accelSpeed;
        }
        else
        {
            horVel = horVel.normalized * maxSpeed;
            rb.velocity = new Vector3(horVel.x, rb.velocity.y, horVel.y);
        }
        RaycastHit hit;
        if ((sidewaysForce == 0 && forwardForce == 0) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x / (friction * Time.deltaTime), rb.velocity.y, rb.velocity.z / (friction * Time.deltaTime));
        }
        grounded = false;
        if (Physics.SphereCast(transform.position, gameObject.GetComponentInChildren<CapsuleCollider>().radius-0.05f, new Vector3(0,-1,0), out hit, raycastDistance))
        {
            if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.name.Contains("lack"))
                {
                    currOn = Material.Black;
                }
                else if (hit.transform.parent.gameObject.name.Contains("hite"))
                {
                    currOn = Material.Black;
                }
                else
                {
                    currOn = Material.None;
                }
            }
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Portal Walls"))
            {
                grounded = true;
            }
            if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.Play(transform, audioClips[(int)currOn][audioClips[(int)currOn].Count - 1], AudioManager.Mixer.SFX, vol * 2, true, transform.position, false);
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
        if (grounded && move && !playing)
        {
            if (currOn != Material.None)
            {
                playing = true;
                AudioManager.Instance.Play(transform, audioClips[(int)currOn][Random.Range(0, audioClips[(int)currOn].Count - 1)], AudioManager.Mixer.SFX, vol, true, transform.position, false);
                StartCoroutine(Wait());
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(dur);
        playing = false;
    }
    public enum Material
    {
        None,
        Black,
        White,
    }
}
