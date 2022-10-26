using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private LevelSystem _levelSystem;

    private int _next;

    private void Awake()
    {
        Switch();
    }

    private void OnEnable()
    {
        _levelSystem.Wave1Finished += Switch;
        _levelSystem.Wave2Finished += Switch;
        _levelSystem.Wave3Finished += Switch;
    }

    private void OnDisable()
    {
        _levelSystem.Wave1Finished -= Switch;
        _levelSystem.Wave2Finished -= Switch;
        _levelSystem.Wave3Finished -= Switch;
    }

    private void Switch()
    {
        foreach (var camera in _cameras)
            camera.enabled = false;

        _cameras[_next].enabled = true;
        _next++;

        if (_next > _cameras.Length - 1)
            _next = 0;
    }
}
