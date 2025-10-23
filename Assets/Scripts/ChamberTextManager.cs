using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChamberTextManager : MonoBehaviour 
{
	private Text text;
	void Start () 
	{
		text = GetComponent<Text>();
		string sceneName = SceneManager.GetActiveScene().name;
		for (int i = 100; i > 0; i--)
		{
			if (!sceneName.Contains(i.ToString() + "_")) { continue; }
			if (i.ToString().Length == 1)
			{
				text.text = "Chamber 0" + i.ToString();
                break;
            }
            text.text = "Chamber " + i.ToString();
            break;
        }
    }
	void Update () 
	{
		
	}
}
