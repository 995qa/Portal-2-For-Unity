using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpManager : MonoBehaviour 
{
    [SerializeField] private AudioSource[] sources;
    private bool hold;
	private Rigidbody heldProp;
    private Vector3 pickupCamRot;
    private Vector3 pickupPropRot;
    private AudioClip holdClip;
    void Start()
    {
        holdClip = sources[1].clip;
    }
    void Update () 
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R))
#else
		if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.X))
#endif
        {
            if (!hold)
			{
                hold = PickUpObject();
            }
			else
			{
                sources[2].Play();
                sources[0].Stop();
                sources[1].Stop();
                sources[1].clip = null;
                sources[1].loop = false;
                hold = false;
                heldProp.useGravity = true;
                heldProp.gameObject.layer = LayerMask.NameToLayer("Physics Prop");
				heldProp = null;
            }
        }
	}
	bool PickUpObject()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.forward,out hit, 3))
		{
            heldProp = hit.rigidbody;
			if (heldProp != null)
			{
                StartCoroutine(HoldAudio());
                heldProp.useGravity = false;
                heldProp.gameObject.layer = LayerMask.NameToLayer("Held Physics Prop");
                pickupCamRot = transform.root.eulerAngles;
                pickupPropRot = heldProp.transform.rotation.eulerAngles;
                return true;
            }
        }
		return false;
	}
	private IEnumerator HoldAudio()
	{
        sources[0].Play();
        yield return new WaitForSeconds(sources[0].clip.length);
        if (hold)
        {
            sources[1].clip = holdClip;
            sources[1].loop = true;
            sources[1].Play();
        }
    }
    void FixedUpdate()
	{
		if (hold)
		{
			heldProp.velocity = Vector3.zero;
			heldProp.angularVelocity = Vector3.zero;
			heldProp.MovePosition(transform.position+(transform.forward*1.2f));
			Vector3 v = new Vector3(
                pickupPropRot.x,
                pickupPropRot.y + (transform.rotation.eulerAngles.y - pickupCamRot.x),
                pickupPropRot.z + (transform.rotation.eulerAngles.z - pickupCamRot.z)
			);
            heldProp.MoveRotation(Quaternion.Euler(v));
		}
	}
}
