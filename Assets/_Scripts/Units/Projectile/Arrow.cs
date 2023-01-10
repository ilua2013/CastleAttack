using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _height = 2f;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Transform target;
    [SerializeField] private Transform _arrowRenderer;

    public Fighter Fighter { get; private set; }

    public event Action<Cell> Reached;

    public void FlyTo(Vector3 target, Fighter fighter, Action onEnd = null, Action onFlyed = null, float delay = 0, Cell cell = null)
    {
        Fighter = fighter;
        StartCoroutine(Fly(target, delay, onEnd, onFlyed, cell));
    }

    private IEnumerator Fly(Vector3 target, float delay = 0, Action onEnd = null, Action onFlyed = null, Cell cell = null)
    {
        yield return new WaitForSeconds(delay);

        transform.parent = null;

        if (cell != null)
            target = cell.transform.position;

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

            if (percent != 1)
                transform.forward = transform.position - deltaPos;

            yield return null;
        }

        if (_arrowRenderer != null)
            _arrowRenderer.localScale = Vector3.zero;

        onFlyed?.Invoke();
        Reached?.Invoke(cell);

        yield return new WaitForSeconds(0.75f);

        onEnd?.Invoke();

        Destroy(gameObject);
    }
}
