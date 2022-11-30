using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
        YandexSDK.Instance.ShowInterstitial();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
