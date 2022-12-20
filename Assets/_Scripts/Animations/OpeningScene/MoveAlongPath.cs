using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPath : MonoBehaviour
{
    private const string JumpState = "Jump";

    [SerializeField] private float _lerpTime = 3f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _distance;

    public void Move(float duration)
    {
        StartCoroutine(MoveTo(_distance, _lerpTime, duration));
    }

    private IEnumerator MoveTo(float distance, float lerpTime, float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            yield return new WaitForSeconds(0.2f);
            _animator.SetTrigger(JumpState);

            Vector3 target = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

            float time = 0;
            float percent = 0;
            Vector3 startPos = transform.position;
            Vector3 targetPos = target - startPos;

            while (percent != 1)
            {
                time += Time.deltaTime;
                elapsed += Time.deltaTime;

                percent = time / lerpTime;
                percent = percent > 1 ? 1 : percent;

                transform.position = startPos + (targetPos * percent);

                yield return null;
            }
        }
    }
}
