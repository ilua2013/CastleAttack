using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    public bool start;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _height = 2f;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Transform target;

    private void Update()
    {
        if (start)
        {
            start = false;
            FlyTo(target.position);
        }
    }

    public void FlyTo(Vector3 target, Action onEnd = null)
    {
        StartCoroutine(Fly(target, onEnd));
    }

    private IEnumerator Fly(Vector3 target, Action onEnd = null)
    {
        Vector3 targetPos = target - transform.position;
        Vector3 startPos = transform.position;
        Vector3 deltaPos;

        float time = 0;
        float percent;
        float timeFly = Vector3.Distance(transform.position, target) / _speed;
        float height = Vector3.Distance(transform.position, target) / _height;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            time = time > timeFly ? timeFly : time + Time.deltaTime;

            percent = time / timeFly;

            deltaPos = transform.position;

            transform.position = startPos + targetPos * percent + new Vector3(0, height * _curve.Evaluate(percent), 0);

            transform.forward = transform.position - deltaPos;
            yield return null;
        }

        onEnd?.Invoke();

        Destroy(gameObject, 2f);
    }
}
