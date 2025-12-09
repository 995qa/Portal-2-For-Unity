using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonManager : MonoBehaviour 
{
	private int touching;
	private ButtonManager button;
	private int prev;
    [HideInInspector] public bool isPressed;
    [HideInInspector] public bool isDown;
    [SerializeField] private Vector3 pressDistance;
    [SerializeField] private float speed;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Texture2D[] texes;
    [SerializeField] private AudioClip[] downUp;
    [SerializeField] private float vol;
    private Transform child;
    private bool down;
    private Vector3 delta;
    private Vector3 init;
    private void Start()
    {
        delta = pressDistance * speed;
        child = transform.GetChild(0).GetChild(0);
        init = child.position;
    }
    public void Press()
    {
        down = true;
        isPressed = true;
        AudioManager.Instance.Play(transform, downUp[0], AudioManager.Mixer.SFX, vol, true, transform.position, false);
    }
    public void Release()
    {
        isPressed = false;
        AudioManager.Instance.Play(transform, downUp[1], AudioManager.Mixer.SFX, vol, true, transform.position, false);
    }
    void Update()
    {
        if (prev == 0 && touching > 0)
        {
            Press();
        }
        if (prev > 0 && touching == 0)
        {
            Release();
        }
        prev = touching;
        if (down)
        {
            child.Translate(delta * Time.deltaTime, Space.World);
        }
        else
        {
            child.Translate(-delta * Time.deltaTime, Space.World);
        }
        if (child.position.y < init.y + pressDistance.y)
        {
            child.position = init + pressDistance;
        }
        else if (child.position.y > init.y)
        {
            child.position = init;
        }
        if (child.position == init + pressDistance && !isPressed)
        {
            down = false;
        }
        if (child.position != init)
        {
            isDown = true;
            mr.sharedMaterials[0].mainTexture = texes[1];
        }
        else
        {
            isDown = false;
            mr.sharedMaterials[0].mainTexture = texes[0];
        }
    }
    public void OnTriggerEnter()
	{
		touching++;
	}
	public void OnTriggerExit()
	{
		touching--;
	}
}
