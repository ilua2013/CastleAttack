using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
#if !UNITY_EDITOR
        YandexSDK.Instance.ShowInterstitial();
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
