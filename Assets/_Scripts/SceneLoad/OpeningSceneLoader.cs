using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneLoader : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;

    /// <summary>
    /// Animation event
    /// </summary>
    public void LoadLevel()
    {
        Saves.SetBool(SaveController.Params.IsOpeningViewed, true);
        Saves.Save();

#if !UNITY_EDITOR
        _sceneLoader.LoadNextLevel();
#else
        _sceneLoader.LoadNextLevel();
#endif
    }
}
