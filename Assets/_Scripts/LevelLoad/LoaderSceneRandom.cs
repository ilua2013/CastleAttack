using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderSceneRandom : MonoBehaviour
{
    [SerializeField] private string[] _scenesName = new string[] { };

    private void Awake()
    {
        SceneManager.LoadScene(_scenesName[Random.Range(0, _scenesName.Length)]);
    }
}
