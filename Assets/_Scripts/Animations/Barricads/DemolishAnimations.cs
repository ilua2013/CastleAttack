using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemolishAnimations : MonoBehaviour
{
    public bool playAnim;
    [SerializeField] private float _duration;
    [SerializeField] private float _delayBeetwenObject;
    [SerializeField] private float _forceObject;

    private List<ForceObject> _forceObjects;

    private void Awake()
    {
        _forceObjects = GetComponentsInChildren<ForceObject>().ToList();
    }

    private void Update()
    {
        if (playAnim)
        {
            playAnim = false;
            Play();
        }
    }

    public void Play(Action onEnd = null)
    {
        StartCoroutine(PlayAnimation(onEnd));
    }

    private IEnumerator PlayAnimation(Action onEnd = null)
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var item in _forceObjects)
        {
            item.Force(_forceObject, Vector3.up * 4 + UnityEngine.Random.insideUnitSphere, transform.position + Vector3.down * 25);

            yield return new WaitForSeconds(_delayBeetwenObject);
        }

        yield return new WaitForSeconds(_duration);

        onEnd?.Invoke();
    }
}
