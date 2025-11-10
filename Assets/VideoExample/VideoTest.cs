using UnityEngine;
public class VideoTest : MonoBehaviour
{
    void Start()
    {
        UnityEngine.N3DS.Video.Play(Application.streamingAssetsPath + "/Rickroll.moflex", N3dsScreen.Top);
    }
}
