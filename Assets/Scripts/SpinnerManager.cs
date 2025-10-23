using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerManager : MonoBehaviour 
{
	[SerializeField] private float speed;
	private Image spinner;
	void Awake()
	{
		spinner = GetComponent<Image>();
        StartCoroutine(Spin());
    }
    private IEnumerator Spin()
	{
        while (true)
        {
            spinner.rectTransform.localEulerAngles += new Vector3(0, 0, -22.5f);
            yield return new WaitForSeconds(1 / speed);
        }
    }
}
