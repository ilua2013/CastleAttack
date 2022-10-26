using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFinisher : MonoBehaviour
{
    public event Action Finished;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Finished?.Invoke();
    }
}
