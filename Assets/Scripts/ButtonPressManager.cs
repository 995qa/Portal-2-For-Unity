using UnityEngine;

public class ButtonPressManager : MonoBehaviour
{
    [SerializeField] private LayerMask button;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PressObject();
        }
    }
    void PressObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3, button))
        {
            if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.GetComponent<ButtonManager>() != null)
                {
                    hit.transform.parent.gameObject.GetComponent<ButtonManager>().Press();
                }
            }
        }
    }
}
