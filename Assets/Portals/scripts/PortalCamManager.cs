using UnityEngine;

public class PortalCamManager : MonoBehaviour 
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private Transform thisPortal;
    private Vector3 DTP;

    void Update() 
	{
        DTP = thisPortal.position - otherPortal.position;
        transform.position = playerCam.position;
        transform.position += DTP;
        transform.rotation = Quaternion.LookRotation(-playerCam.position + otherPortal.position);
        float distance = (playerCam.position - otherPortal.position).magnitude;
        Debug.Log(this.name + ": " + distance);
        this.GetComponent<Camera>().nearClipPlane = distance-0.3f;
        this.GetComponent<Camera>().fieldOfView = 60 / (distance + 0.15f);
    }
}
