using UnityEngine;
using UnityEngine.UI;

public class BottomScreenManager : MonoBehaviour 
{
	[SerializeField] private Canvas canvas;
    [SerializeField] private Image[] dotImages;
    [SerializeField] private Sprite[] dotSprites;
    public int dots;
    public static BottomScreenManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start () 
	{
		canvas.gameObject.SetActive(false);
	}
	void Update () 
	{
		if (TransitionManager.Instance.bottomScreen&&!canvas.gameObject.activeInHierarchy)
		{
            canvas.gameObject.SetActive(true);
        }
		else if (!TransitionManager.Instance.bottomScreen && canvas.gameObject.activeInHierarchy)
        {
            canvas.gameObject.SetActive(false);
        }
		if (canvas.gameObject.activeInHierarchy)
		{
            for (int i = 0; i < dotImages.Length; i++)
            {
                if (i < dots)
                {
                    dotImages[i].sprite = dotSprites[1];
                }
                else
                {
                    dotImages[i].sprite = dotSprites[0];
                }
            }
		}
    }
}
