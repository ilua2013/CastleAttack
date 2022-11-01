using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private WallDamage _wallDamage;

    private void OnEnable()
    {
        _wallDamage.StopedGame += EnableCanvas;
    }

    private void OnDisable()
    {
        _wallDamage.StopedGame -= EnableCanvas;
    }

    private void EnableCanvas()
    {
        _canvas.SetActive(true);
        Time.timeScale = 0;
    }
}
