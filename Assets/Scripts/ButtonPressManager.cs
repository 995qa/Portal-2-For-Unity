using UnityEngine;

public class ButtonPressManager : MonoBehaviour
{
    [SerializeField] private LayerMask button;
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
#else
		if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.X))
#endif
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
