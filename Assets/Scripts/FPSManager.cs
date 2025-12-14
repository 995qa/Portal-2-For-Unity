using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour 
{
	[SerializeField] private RectTransform bar;
	[SerializeField] Image image;
	private bool show;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.F2))
        {
			show = !show;
			if (show)
			{
				image.color = new Color(255,255, 255, 255);
			}
			else
			{
                image.color = new Color(255, 255, 255, 0);
            }
        }
		bar.sizeDelta = new Vector2(bar.sizeDelta.x, 1/Time.smoothDeltaTime);
	}
}
