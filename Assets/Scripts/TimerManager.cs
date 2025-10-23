using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour 
{
	private Text text;
	private float time;
	void Start()
	{
        text = GetComponent<Text>();
		time = 0;
	}
	void Update () 
	{
		time += Time.unscaledDeltaTime;
		string hours = ((int)(time / 360)).ToString();
        string minutes = ((int)((time / 60) % 60)).ToString();
        string seconds = ((int)(time % 60)).ToString();
        string milliseconds = ((int)((time-(int)time)*100)).ToString();
        if (hours.Length == 1)
        {
            hours = "0" + hours;
        }
        if (minutes.Length == 1)
        {
            minutes = "0" + minutes;
        }
        if (seconds.Length == 1)
        {
            seconds = "0" + seconds;
        }
        if (milliseconds.Length == 1)
        {
            milliseconds = "0" + milliseconds;
        }
        text.text = hours+":"+minutes+":"+seconds+":"+milliseconds;
		
	}
}
