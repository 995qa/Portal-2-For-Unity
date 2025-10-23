using UnityEngine;
using UnityEngine.UI;

public class MainMenuBottomScreenManager : MonoBehaviour 
{
    public Text previous;
    public Text main;
    public Text next;
    public bool update;
    //==============================SINGLETON==============================
    public static MainMenuBottomScreenManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one " + this.name + ", ya chump");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    //=====================================================================
}
