using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 fadeInHoldFadeOut;
    [SerializeField] private Image[] fade;
    [SerializeField] private Image[] image;
    [SerializeField] private AudioClip c;
    public bool bottomScreen;
    private float progess;
    public static TransitionManager Instance
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
        StartCoroutine(FadeIn());
    }
    private void Start()
    {
        AudioManager.Instance.Play(transform, c, AudioManager.Mixer.Music);
    }
    public void Scene(string name, Sprite image = null, bool useBottomScreen = false, Sprite bottomImage = null)
    {
        if (image == null)
        {
            StartCoroutine(LoadScene(name));
        }
        else
        {
            StartCoroutine(LoadSceneImage(name, image, useBottomScreen, bottomImage));
        }
    }
    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1 - (timer / fadeInHoldFadeOut.z));
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1 - (timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
    private IEnumerator LoadScene(string name)
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, timer / fadeInHoldFadeOut.x);
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }
        LoadSceneAsync(name);
        while (SceneManager.GetActiveScene().name != name)
        {
            yield return null;
        }
        timer = 0;
        while (timer < fadeInHoldFadeOut.y)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1);
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1);
            yield return null;
        }
        timer = 0;
        while (timer < fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1 - (timer / fadeInHoldFadeOut.z));
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1 - (timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
    private IEnumerator LoadSceneImage(string name, Sprite imageSprite, bool useBottomScreen, Sprite bottomImage)
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, timer / fadeInHoldFadeOut.x);
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }

        image[0].sprite = imageSprite;
        image[0].color = new Color(255, 255, 255, 255);
        if (bottomImage != null)
        {
            image[1].sprite = bottomImage;
            image[1].color = new Color(255, 255, 255, 255);
        }
        if (useBottomScreen)
        {
            bottomScreen = true;
        }


        timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1 - (timer / fadeInHoldFadeOut.x));
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1 - (timer / fadeInHoldFadeOut.x));
            yield return null;
        }

        LoadSceneAsync(name);
        while (SceneManager.GetActiveScene().name != name)
        {
            if (useBottomScreen)
            {
                BottomScreenManager.Instance.dots = (int)(progess * 19);
            }
            yield return null;
        }


        timer = 0;
        while (timer < fadeInHoldFadeOut.y)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1);
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1);
            yield return null;
        }


        timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, timer / fadeInHoldFadeOut.x);
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, timer / fadeInHoldFadeOut.x);
            yield return null;
        }


        image[0].color = new Color(255, 255, 255, 0);
        image[1].color = new Color(255, 255, 255, 0);
        if (useBottomScreen)
        {
            bottomScreen = false;
        }


        timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade[0].color = new Color(fade[0].color.r, fade[0].color.g, fade[0].color.b, 1 - (timer / fadeInHoldFadeOut.x));
            fade[1].color = new Color(fade[1].color.r, fade[1].color.g, fade[1].color.b, 1 - (timer / fadeInHoldFadeOut.x));
            yield return null;
        }
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            progess = asyncLoad.progress;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

}
