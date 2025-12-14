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
    [SerializeField] private float dur;

    private Rigidbody rb;
    private Transform player;
    private float angle;
    private bool grounded;
    private bool playing;
    private bool move;

    private Material currOn;
    private List<AudioClip>[] audioClips;

    public static PlayerController3D Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one " + name + ", ya chump");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        player = transform;
        rb = GetComponent<Rigidbody>();

        audioClips = new List<AudioClip>[2];
        audioClips[0] = wAudioClips.ToList();
        audioClips[1] = bAudioClips.ToList();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleGroundCheck();
    }

    private void HandleLook()
    {
        angle = cam.rotation.eulerAngles.x;
        if (angle > 180) angle -= 360;

 //       if (Input.GetMouseButton(0))
 //       {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed * 2f;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * 2f;

            player.rotation = Quaternion.Euler(
                0f,
                player.rotation.eulerAngles.y + mouseX,
                0f
            );

            float nextCamAngle = angle - mouseY;
            nextCamAngle = Mathf.Clamp(nextCamAngle, -90f, 90f);

            cam.rotation = Quaternion.Euler(
                nextCamAngle,
                cam.rotation.eulerAngles.y,
                0f
            );
//        }

#if UNITY_EDITOR
        float camAngle = 0f;
        float playerAngle = 0f;

        if (Input.GetKey(KeyCode.RightArrow)) playerAngle += lookSpeed / 5f;
        if (Input.GetKey(KeyCode.LeftArrow)) playerAngle -= lookSpeed / 5f;
        if (Input.GetKey(KeyCode.UpArrow)) camAngle += lookSpeed / 5f;
        if (Input.GetKey(KeyCode.DownArrow)) camAngle -= lookSpeed / 5f;

        rb.freezeRotation = false;
        player.rotation = Quaternion.Euler(0f, player.rotation.eulerAngles.y + playerAngle, 0f);
        rb.freezeRotation = true;

        float nextAngle = Mathf.Clamp(angle - camAngle, -90f, 90f);
        cam.rotation = Quaternion.Euler(nextAngle, cam.rotation.eulerAngles.y, 0f);
#endif
    }

    private void HandleMovement()
    {
        Vector2 horVel = new Vector2(rb.velocity.x, rb.velocity.z);

        float forwardForce = 0f;
        float sidewaysForce = 0f;
        move = false;

        if (Input.GetKey(KeyCode.W)) { forwardForce += Time.deltaTime; move = true; }
        if (Input.GetKey(KeyCode.S)) { forwardForce -= Time.deltaTime; move = true; }
        if (Input.GetKey(KeyCode.D)) { sidewaysForce += Time.deltaTime; move = true; }
        if (Input.GetKey(KeyCode.A)) { sidewaysForce -= Time.deltaTime; move = true; }

        PortalGunAnimator.Instance.animate = move;

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

        if (!move && grounded)
        {
            rb.velocity = new Vector3(
                rb.velocity.x / (friction * Time.deltaTime),
                rb.velocity.y,
                rb.velocity.z / (friction * Time.deltaTime)
            );
        }
    }

    private void HandleGroundCheck()
    {
        grounded = false;
        RaycastHit hit;

        float radius = GetComponentInChildren<CapsuleCollider>().radius - 0.05f;

        if (Physics.SphereCast(transform.position, radius, Vector3.down, out hit, raycastDistance))
        {
            if (hit.transform.parent != null)
            {
                if (hit.transform.parent.name.Contains("lack"))
                    currOn = Material.Black;
                else if (hit.transform.parent.name.Contains("hite"))
                    currOn = Material.White;
                else
                    currOn = Material.None;
            }

            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Portal Walls"))
                grounded = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.Play(
                    transform,
                    audioClips[(int)currOn][audioClips[(int)currOn].Count - 1],
                    AudioManager.Mixer.SFX,
                    vol * 2,
                    true,
                    transform.position,
                    false
                );

                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (grounded && move && !playing && currOn != Material.None)
        {
            playing = true;
            AudioManager.Instance.Play(
                transform,
                audioClips[(int)currOn][Random.Range(0, audioClips[(int)currOn].Count - 1)],
                AudioManager.Mixer.SFX,
                vol,
                true,
                transform.position,
                false
            );

            StartCoroutine(Wait());
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
        White
    }
}
