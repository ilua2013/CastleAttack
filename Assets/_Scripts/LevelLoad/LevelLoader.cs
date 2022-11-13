using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader
{
    public AsyncOperation LoadScene(string name)
    {
        return SceneManager.LoadSceneAsync(name);
    }
}
