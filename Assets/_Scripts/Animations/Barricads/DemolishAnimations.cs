using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemolishAnimations : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private ParticleSystem _vfx;

    private List<ForceObject> _forceObjects;

    private void Awake()
    {
        _forceObjects = GetComponentsInChildren<ForceObject>().ToList();

        foreach (var obj in _forceObjects)
            obj.Rigidbody.isKinematic = true;
    }

    public void Play(Action onEnd = null)
    {
        StartCoroutine(PlayAnimation(onEnd));
    }

    private IEnumerator PlayAnimation(Action onEnd = null)
    {
        yield return new WaitForSeconds(0.5f);
        _vfx.Play();

        foreach (var obj in _forceObjects)
        {
            obj.Rigidbody.isKinematic = false;
            obj.Force(UnityEngine.Random.Range(100f, 120f), Vector3.up * 7 + UnityEngine.Random.insideUnitSphere);
        }

        yield return new WaitForSeconds(_duration);

        onEnd?.Invoke();
    }
}
