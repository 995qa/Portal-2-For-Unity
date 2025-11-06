using UnityEngine;

public class beer : MonoBehaviour 
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            col.transform.parent.gameObject.GetComponent<PlayerController3D>().drunk = !col.transform.parent.gameObject.GetComponent<PlayerController3D>().drunk;
            Physics.gravity = -Physics.gravity;
        }
    }
}
