using UnityEngine;

public class PortalCamManager : MonoBehaviour 
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private Transform thisPortal;
    private Vector3 DTP;
    private Quaternion DTR;

    void Update() 
	{
        Quaternion thisRot = thisPortal.rotation;
        Quaternion otherRot = otherPortal.rotation;
        DTP = thisPortal.position - otherPortal.position;
        DTR = thisRot * Quaternion.Inverse(otherRot);
        transform.position = playerCam.position;
        transform.position += DTP;
        transform.rotation = Quaternion.LookRotation(playerCam.position - otherPortal.position);
        transform.rotation *= DTR;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        float distance = (playerCam.position - otherPortal.position).magnitude;
        this.GetComponent<Camera>().nearClipPlane = distance - 0.3f;
        this.GetComponent<Camera>().fieldOfView = 60 / (distance + 0.141f);
    }
}
