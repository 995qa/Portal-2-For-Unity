using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootloader : MonoBehaviour 
{
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] gameScenes;
    [SerializeField] private string mainMenu;
    [SerializeField] private Sprite[] gameLoadingScreens;
    [SerializeField] private Sprite[] menuLoadingScreens;
    public bool N3DSMode;
    private int state;
    public static Bootloader Instance
    {
        get; private set;
    }
    private void Update()
    {
#if !UNITY_EDITOR
        if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.ZL) && UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.ZR))
#else
		if (Input.GetKeyDown(KeyCode.F1))
#endif
        {
            Debug.Log("uibgr");
            SceneManager.LoadScene(scenes[state]);
            state++;
            state %= scenes.Length;
        }
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
        DontDestroyOnLoad(gameObject);
        N3DSMode = UnityEngine.N3DS.Application.isRunningOnSnake;
    }
    public void NextScene()
    {
        if (gameScenes.Contains<string>(scenes[state]))
        {
            if (scenes[state].Contains("N3DS") && !N3DSMode)
            {
                state++;
            }
            Sprite image = gameLoadingScreens[Random.Range(0, gameLoadingScreens.Length)];
            TransitionManager.Instance.Scene(scenes[state], image, true, image);
        }
        else if (mainMenu == scenes[state])
        {
            Sprite image = menuLoadingScreens[Random.Range(0, menuLoadingScreens.Length)];
            TransitionManager.Instance.Scene(scenes[state], image, false, image);
        }
        else
        {
            TransitionManager.Instance.Scene(scenes[state]);
        }
        state++;
        state %= scenes.Length;
    }
}
