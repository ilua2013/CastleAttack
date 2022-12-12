using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookCamera : MonoBehaviour
{
    private Camera _camera;

    private void OnValidate()
    {
        if (Camera.main == null)
            return;
        _camera = Camera.main;
        Vector3 targetLook = _camera.transform.position - transform.position;
        targetLook.x = 0;

        //transform.LookAt(transform.position * 2 - _camera.transform.position);
        transform.forward = -targetLook;
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 targetLook = _camera.transform.position - transform.position;
        targetLook.x = 0;

        //transform.LookAt(transform.position * 2 - _camera.transform.position);
        transform.forward = -targetLook;
    }
}
