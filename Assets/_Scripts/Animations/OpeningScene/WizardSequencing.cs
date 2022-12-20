using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSequencing : MonoBehaviour
{
    [SerializeField] private MoveAlongPath _moveAlongPath;
    [SerializeField] private float[] _durations;
    [SerializeField] private Transform[] _checkpoints;

    private Action[] _sequences = new Action[3];

    private void Awake()
    {
        _sequences[0] = () => _moveAlongPath.Move(_durations[0]);
    }

    private void OnEnable()
    {
        Run();
    }

    private void Run()
    {
        StartCoroutine(DoSequences());
    }

    private IEnumerator DoSequences()
    {
        for (int i = 0; i < _sequences.Length; i++)
        {
            _sequences[i]?.Invoke();

            yield return new WaitForSeconds(_durations[i]);
        }
    }
}
