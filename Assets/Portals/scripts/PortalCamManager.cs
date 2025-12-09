using UnityEngine;

public class PortalCamManager : MonoBehaviour 
{
    private Transform playerCam;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private Transform thisPortal;
    private Vector3 DTP;
    private Quaternion DTR;
    private void Start()
    {
        playerCam = Camera.main.transform;
    }
    void Update() 
	{
        transform.position = playerCam.position;
        transform.rotation = Quaternion.LookRotation(thisPortal.position - playerCam.position);
        transform.parent.gameObject.GetComponent<PortalTrigger>().Teleport(gameObject);
        float distance = (transform.position - otherPortal.position).magnitude;
        this.GetComponent<Camera>().nearClipPlane = distance - 0.3f;
        this.GetComponent<Camera>().fieldOfView = 60 / (distance + 0.141f);
    }
}
